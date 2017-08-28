using System;

using AppKit;
using Foundation;

using Chat.Connection.Grpc;

namespace Chat.Client.Mac
{
    public partial class ViewController : NSViewController
    {
        Client client;

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Do any additional setup after loading the view.
        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }

        partial void Send(NSButton sender)
        {
            var text = MessageField.StringValue;
            client.SendTextMessage(text);
        }

        partial void Start(NSButton sender)
        {
            var target = TargetField.StringValue;
            var host = HostField.StringValue;
            var port = PortField.IntValue;
            var userid = UseridField.IntValue;
            var password = PasswordField.StringValue;

            var builder = new ClientBuilder()
                .UseGrpc(target, host, port)
                .SetUser(userid, password);
            client = builder.Build();
            client.Login();
        }

        partial void Close(NSButton sender)
        {

        }
    }
}
