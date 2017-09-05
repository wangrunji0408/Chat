// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: Protobuf.proto
#pragma warning disable 1591
#region Designer generated code

using System;
using System.Threading;
using System.Threading.Tasks;
using grpc = global::Grpc.Core;

namespace Chat.Connection.Grpc {
  public static partial class ChatServerService
  {
    static readonly string __ServiceName = "chat.grpc.ChatServerService";

    static readonly grpc::Marshaller<global::Chat.Core.Models.LoginRequest> __Marshaller_LoginRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Chat.Core.Models.LoginRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Chat.Core.Models.LoginResponse> __Marshaller_LoginResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Chat.Core.Models.LoginResponse.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Chat.Connection.Grpc.RegisterAddressRequest> __Marshaller_RegisterAddressRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Chat.Connection.Grpc.RegisterAddressRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Chat.Core.Models.Response> __Marshaller_Response = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Chat.Core.Models.Response.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Chat.Core.Models.SignupRequest> __Marshaller_SignupRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Chat.Core.Models.SignupRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Chat.Core.Models.SignupResponse> __Marshaller_SignupResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Chat.Core.Models.SignupResponse.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Chat.Core.Models.SendMessageRequest> __Marshaller_SendMessageRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Chat.Core.Models.SendMessageRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Chat.Core.Models.SendMessageResponse> __Marshaller_SendMessageResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Chat.Core.Models.SendMessageResponse.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Chat.Core.Models.GetMessagesRequest> __Marshaller_GetMessagesRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Chat.Core.Models.GetMessagesRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Chat.Core.Models.ChatMessage> __Marshaller_ChatMessage = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Chat.Core.Models.ChatMessage.Parser.ParseFrom);

    static readonly grpc::Method<global::Chat.Core.Models.LoginRequest, global::Chat.Core.Models.LoginResponse> __Method_Login = new grpc::Method<global::Chat.Core.Models.LoginRequest, global::Chat.Core.Models.LoginResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Login",
        __Marshaller_LoginRequest,
        __Marshaller_LoginResponse);

    static readonly grpc::Method<global::Chat.Connection.Grpc.RegisterAddressRequest, global::Chat.Core.Models.Response> __Method_RegisterAddress = new grpc::Method<global::Chat.Connection.Grpc.RegisterAddressRequest, global::Chat.Core.Models.Response>(
        grpc::MethodType.Unary,
        __ServiceName,
        "RegisterAddress",
        __Marshaller_RegisterAddressRequest,
        __Marshaller_Response);

    static readonly grpc::Method<global::Chat.Core.Models.SignupRequest, global::Chat.Core.Models.SignupResponse> __Method_Signup = new grpc::Method<global::Chat.Core.Models.SignupRequest, global::Chat.Core.Models.SignupResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Signup",
        __Marshaller_SignupRequest,
        __Marshaller_SignupResponse);

    static readonly grpc::Method<global::Chat.Core.Models.SendMessageRequest, global::Chat.Core.Models.SendMessageResponse> __Method_SendMessage = new grpc::Method<global::Chat.Core.Models.SendMessageRequest, global::Chat.Core.Models.SendMessageResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "SendMessage",
        __Marshaller_SendMessageRequest,
        __Marshaller_SendMessageResponse);

    static readonly grpc::Method<global::Chat.Core.Models.GetMessagesRequest, global::Chat.Core.Models.ChatMessage> __Method_GetMessages = new grpc::Method<global::Chat.Core.Models.GetMessagesRequest, global::Chat.Core.Models.ChatMessage>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "GetMessages",
        __Marshaller_GetMessagesRequest,
        __Marshaller_ChatMessage);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Chat.Connection.Grpc.ProtobufReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of ChatServerService</summary>
    public abstract partial class ChatServerServiceBase
    {
      public virtual global::System.Threading.Tasks.Task<global::Chat.Core.Models.LoginResponse> Login(global::Chat.Core.Models.LoginRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      /// <summary>
      ///rpc Logout (Request) returns (Response);
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>The response to send back to the client (wrapped by a task).</returns>
      public virtual global::System.Threading.Tasks.Task<global::Chat.Core.Models.Response> RegisterAddress(global::Chat.Connection.Grpc.RegisterAddressRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::Chat.Core.Models.SignupResponse> Signup(global::Chat.Core.Models.SignupRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::Chat.Core.Models.SendMessageResponse> SendMessage(global::Chat.Core.Models.SendMessageRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      /// <summary>
      ///rpc MakeFriend (MakeFriendRequest) returns (MakeFriendResponse);
      ///rpc GetPeoplesInfo (GetPeoplesInfoRequest) returns (stream People);
      ///rpc NewChatroom (NewChatroomRequest) returns (ChatroomResponse);
      ///rpc GetChatroom (GetChatroomRequest) returns (ChatroomResponse);
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="responseStream">Used for sending responses back to the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>A task indicating completion of the handler.</returns>
      public virtual global::System.Threading.Tasks.Task GetMessages(global::Chat.Core.Models.GetMessagesRequest request, grpc::IServerStreamWriter<global::Chat.Core.Models.ChatMessage> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for ChatServerService</summary>
    public partial class ChatServerServiceClient : grpc::ClientBase<ChatServerServiceClient>
    {
      /// <summary>Creates a new client for ChatServerService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public ChatServerServiceClient(grpc::Channel channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for ChatServerService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public ChatServerServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected ChatServerServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected ChatServerServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual global::Chat.Core.Models.LoginResponse Login(global::Chat.Core.Models.LoginRequest request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return Login(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Chat.Core.Models.LoginResponse Login(global::Chat.Core.Models.LoginRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Login, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Chat.Core.Models.LoginResponse> LoginAsync(global::Chat.Core.Models.LoginRequest request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return LoginAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Chat.Core.Models.LoginResponse> LoginAsync(global::Chat.Core.Models.LoginRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Login, null, options, request);
      }
      /// <summary>
      ///rpc Logout (Request) returns (Response);
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::Chat.Core.Models.Response RegisterAddress(global::Chat.Connection.Grpc.RegisterAddressRequest request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return RegisterAddress(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      ///rpc Logout (Request) returns (Response);
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::Chat.Core.Models.Response RegisterAddress(global::Chat.Connection.Grpc.RegisterAddressRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_RegisterAddress, null, options, request);
      }
      /// <summary>
      ///rpc Logout (Request) returns (Response);
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::Chat.Core.Models.Response> RegisterAddressAsync(global::Chat.Connection.Grpc.RegisterAddressRequest request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return RegisterAddressAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      ///rpc Logout (Request) returns (Response);
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::Chat.Core.Models.Response> RegisterAddressAsync(global::Chat.Connection.Grpc.RegisterAddressRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_RegisterAddress, null, options, request);
      }
      public virtual global::Chat.Core.Models.SignupResponse Signup(global::Chat.Core.Models.SignupRequest request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return Signup(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Chat.Core.Models.SignupResponse Signup(global::Chat.Core.Models.SignupRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Signup, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Chat.Core.Models.SignupResponse> SignupAsync(global::Chat.Core.Models.SignupRequest request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return SignupAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Chat.Core.Models.SignupResponse> SignupAsync(global::Chat.Core.Models.SignupRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Signup, null, options, request);
      }
      public virtual global::Chat.Core.Models.SendMessageResponse SendMessage(global::Chat.Core.Models.SendMessageRequest request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return SendMessage(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Chat.Core.Models.SendMessageResponse SendMessage(global::Chat.Core.Models.SendMessageRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_SendMessage, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Chat.Core.Models.SendMessageResponse> SendMessageAsync(global::Chat.Core.Models.SendMessageRequest request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return SendMessageAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Chat.Core.Models.SendMessageResponse> SendMessageAsync(global::Chat.Core.Models.SendMessageRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_SendMessage, null, options, request);
      }
      /// <summary>
      ///rpc MakeFriend (MakeFriendRequest) returns (MakeFriendResponse);
      ///rpc GetPeoplesInfo (GetPeoplesInfoRequest) returns (stream People);
      ///rpc NewChatroom (NewChatroomRequest) returns (ChatroomResponse);
      ///rpc GetChatroom (GetChatroomRequest) returns (ChatroomResponse);
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncServerStreamingCall<global::Chat.Core.Models.ChatMessage> GetMessages(global::Chat.Core.Models.GetMessagesRequest request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return GetMessages(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      ///rpc MakeFriend (MakeFriendRequest) returns (MakeFriendResponse);
      ///rpc GetPeoplesInfo (GetPeoplesInfoRequest) returns (stream People);
      ///rpc NewChatroom (NewChatroomRequest) returns (ChatroomResponse);
      ///rpc GetChatroom (GetChatroomRequest) returns (ChatroomResponse);
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncServerStreamingCall<global::Chat.Core.Models.ChatMessage> GetMessages(global::Chat.Core.Models.GetMessagesRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncServerStreamingCall(__Method_GetMessages, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override ChatServerServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new ChatServerServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(ChatServerServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_Login, serviceImpl.Login)
          .AddMethod(__Method_RegisterAddress, serviceImpl.RegisterAddress)
          .AddMethod(__Method_Signup, serviceImpl.Signup)
          .AddMethod(__Method_SendMessage, serviceImpl.SendMessage)
          .AddMethod(__Method_GetMessages, serviceImpl.GetMessages).Build();
    }

  }
  public static partial class ChatClientService
  {
    static readonly string __ServiceName = "chat.grpc.ChatClientService";

    static readonly grpc::Marshaller<global::Chat.Core.Models.SendMessageRequest> __Marshaller_SendMessageRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Chat.Core.Models.SendMessageRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Chat.Core.Models.SendMessageResponse> __Marshaller_SendMessageResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Chat.Core.Models.SendMessageResponse.Parser.ParseFrom);

    static readonly grpc::Method<global::Chat.Core.Models.SendMessageRequest, global::Chat.Core.Models.SendMessageResponse> __Method_NewMessage = new grpc::Method<global::Chat.Core.Models.SendMessageRequest, global::Chat.Core.Models.SendMessageResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "NewMessage",
        __Marshaller_SendMessageRequest,
        __Marshaller_SendMessageResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Chat.Connection.Grpc.ProtobufReflection.Descriptor.Services[1]; }
    }

    /// <summary>Base class for server-side implementations of ChatClientService</summary>
    public abstract partial class ChatClientServiceBase
    {
      public virtual global::System.Threading.Tasks.Task<global::Chat.Core.Models.SendMessageResponse> NewMessage(global::Chat.Core.Models.SendMessageRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for ChatClientService</summary>
    public partial class ChatClientServiceClient : grpc::ClientBase<ChatClientServiceClient>
    {
      /// <summary>Creates a new client for ChatClientService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public ChatClientServiceClient(grpc::Channel channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for ChatClientService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public ChatClientServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected ChatClientServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected ChatClientServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual global::Chat.Core.Models.SendMessageResponse NewMessage(global::Chat.Core.Models.SendMessageRequest request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return NewMessage(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Chat.Core.Models.SendMessageResponse NewMessage(global::Chat.Core.Models.SendMessageRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_NewMessage, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Chat.Core.Models.SendMessageResponse> NewMessageAsync(global::Chat.Core.Models.SendMessageRequest request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return NewMessageAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Chat.Core.Models.SendMessageResponse> NewMessageAsync(global::Chat.Core.Models.SendMessageRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_NewMessage, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override ChatClientServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new ChatClientServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(ChatClientServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_NewMessage, serviceImpl.NewMessage).Build();
    }

  }
}
#endregion
