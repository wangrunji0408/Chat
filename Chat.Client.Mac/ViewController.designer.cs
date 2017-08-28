// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Chat.Client.Mac
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSTextField HostField { get; set; }

		[Outlet]
		AppKit.NSTextField MessageField { get; set; }

		[Outlet]
		AppKit.NSSecureTextField PasswordField { get; set; }

		[Outlet]
		AppKit.NSTextField PortField { get; set; }

		[Outlet]
		AppKit.NSTextField StatusField { get; set; }

		[Outlet]
		AppKit.NSTextField TargetField { get; set; }

		[Outlet]
		AppKit.NSTextField UseridField { get; set; }

		[Action ("Close:")]
		partial void Close (AppKit.NSButton sender);

		[Action ("Send:")]
		partial void Send (AppKit.NSButton sender);

		[Action ("Start:")]
		partial void Start (AppKit.NSButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (HostField != null) {
				HostField.Dispose ();
				HostField = null;
			}

			if (MessageField != null) {
				MessageField.Dispose ();
				MessageField = null;
			}

			if (PortField != null) {
				PortField.Dispose ();
				PortField = null;
			}

			if (StatusField != null) {
				StatusField.Dispose ();
				StatusField = null;
			}

			if (TargetField != null) {
				TargetField.Dispose ();
				TargetField = null;
			}

			if (UseridField != null) {
				UseridField.Dispose ();
				UseridField = null;
			}

			if (PasswordField != null) {
				PasswordField.Dispose ();
				PasswordField = null;
			}
		}
	}
}
