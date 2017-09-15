using System;
using System.Linq;
using System.Reflection;
using CommandLine;
using Microsoft.Extensions.Logging;

namespace Chat.Client.ConsoleApp.Options.Me
{
    [Verb("me")]
    class MeOption: RootOptionBase
    {
        [Value(0, Required = true)]
        public long PeopleId { get; set; }


        protected override string Str => "Me";
        protected override Type BaseOptionType => typeof(MeOptionBase);

        internal override void Execute(Program app)
        {
            ReadCommands(app);
        }
    }
    
    abstract class MeOptionBase : OptionBase
    {
    }
}