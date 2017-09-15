using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Chat.Core.Models;
using Google.Protobuf;

namespace Chat.Server.Infrastructure
{
    public static class DataService
    {
        public static BinaryReader ReadFile(string fileName)
        {
            var file = File.OpenRead(fileName);
            return new BinaryReader(file);
        }

        public static BinaryReader TestData(int seed, int length)
        {
            const int bufferSize = 0x100;
            var random = new Random(seed);
            var buffer = new byte[bufferSize];
            var stream = new MemoryStream();
            for(int rest = length; rest > 0; rest -= bufferSize)
            {
                random.NextBytes(buffer);
                stream.Write(buffer, 0, Math.Min(rest, bufferSize));
            }
            stream.Position = 0;
            return new BinaryReader(stream);
        }

        public static IAsyncEnumerable<GetDataResponse> GetDataAsync(BinaryReader reader)
        {
            return GetData(reader).ToAsyncEnumerable();
        }
        
        public static IEnumerable<GetDataResponse> GetData(BinaryReader reader)
        {
            const int blockSize = 0x1000;
            var buffer = new byte[blockSize];
            var numread = 0;
            while ((numread = reader.Read(buffer, 0, blockSize)) != 0)
            {
                if(numread != blockSize)
                    Array.Resize(ref buffer, numread);
                yield return new GetDataResponse
                {
                    Data = ByteString.CopyFrom(buffer)
                };
            }
            reader.Close();
        }
    }
}