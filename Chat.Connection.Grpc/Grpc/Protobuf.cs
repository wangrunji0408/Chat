// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: Protobuf.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Chat.Connection.Grpc {

  /// <summary>Holder for reflection information generated from Protobuf.proto</summary>
  public static partial class ProtobufReflection {

    #region Descriptor
    /// <summary>File descriptor for Protobuf.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static ProtobufReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cg5Qcm90b2J1Zi5wcm90bxIJY2hhdC5ncnBjGgxNb2RlbHMucHJvdG8iOQoW",
            "UmVnaXN0ZXJBZGRyZXNzUmVxdWVzdBIOCgZ1c2VySWQYASABKAMSDwoHYWRk",
            "cmVzcxgCIAEoCTLCAgoRQ2hhdFNlcnZlclNlcnZpY2USMAoFTG9naW4SEi5j",
            "aGF0LkxvZ2luUmVxdWVzdBoTLmNoYXQuTG9naW5SZXNwb25zZRJECg9SZWdp",
            "c3RlckFkZHJlc3MSIS5jaGF0LmdycGMuUmVnaXN0ZXJBZGRyZXNzUmVxdWVz",
            "dBoOLmNoYXQuUmVzcG9uc2USMwoGU2lnbnVwEhMuY2hhdC5TaWdudXBSZXF1",
            "ZXN0GhQuY2hhdC5TaWdudXBSZXNwb25zZRJCCgtTZW5kTWVzc2FnZRIYLmNo",
            "YXQuU2VuZE1lc3NhZ2VSZXF1ZXN0GhkuY2hhdC5TZW5kTWVzc2FnZVJlc3Bv",
            "bnNlEjwKC0dldE1lc3NhZ2VzEhguY2hhdC5HZXRNZXNzYWdlc1JlcXVlc3Qa",
            "ES5jaGF0LkNoYXRNZXNzYWdlMAEyVgoRQ2hhdENsaWVudFNlcnZpY2USQQoK",
            "TmV3TWVzc2FnZRIYLmNoYXQuU2VuZE1lc3NhZ2VSZXF1ZXN0GhkuY2hhdC5T",
            "ZW5kTWVzc2FnZVJlc3BvbnNlQheqAhRDaGF0LkNvbm5lY3Rpb24uR3JwY2IG",
            "cHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Chat.Core.Models.ModelsReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Chat.Connection.Grpc.RegisterAddressRequest), global::Chat.Connection.Grpc.RegisterAddressRequest.Parser, new[]{ "UserId", "Address" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class RegisterAddressRequest : pb::IMessage<RegisterAddressRequest> {
    private static readonly pb::MessageParser<RegisterAddressRequest> _parser = new pb::MessageParser<RegisterAddressRequest>(() => new RegisterAddressRequest());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<RegisterAddressRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Chat.Connection.Grpc.ProtobufReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RegisterAddressRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RegisterAddressRequest(RegisterAddressRequest other) : this() {
      userId_ = other.userId_;
      address_ = other.address_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RegisterAddressRequest Clone() {
      return new RegisterAddressRequest(this);
    }

    /// <summary>Field number for the "userId" field.</summary>
    public const int UserIdFieldNumber = 1;
    private long userId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long UserId {
      get { return userId_; }
      set {
        userId_ = value;
      }
    }

    /// <summary>Field number for the "address" field.</summary>
    public const int AddressFieldNumber = 2;
    private string address_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Address {
      get { return address_; }
      set {
        address_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as RegisterAddressRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(RegisterAddressRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (UserId != other.UserId) return false;
      if (Address != other.Address) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (UserId != 0L) hash ^= UserId.GetHashCode();
      if (Address.Length != 0) hash ^= Address.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (UserId != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(UserId);
      }
      if (Address.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Address);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (UserId != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(UserId);
      }
      if (Address.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Address);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(RegisterAddressRequest other) {
      if (other == null) {
        return;
      }
      if (other.UserId != 0L) {
        UserId = other.UserId;
      }
      if (other.Address.Length != 0) {
        Address = other.Address;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            UserId = input.ReadInt64();
            break;
          }
          case 18: {
            Address = input.ReadString();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
