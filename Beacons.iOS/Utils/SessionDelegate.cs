using System;
using MultipeerConnectivity;
using UIKit;
using Foundation;

namespace Beacons.iOS
{
	class MySessionDelegate : MCSessionDelegate
	{
		public override void DidChangeState (MCSession session, MCPeerID peerID, MCSessionState state)
		{
			switch (state) {
			case MCSessionState.Connected:
				Console.WriteLine ("Connected: {0}", peerID.DisplayName);
				break;
			case MCSessionState.Connecting:
				Console.WriteLine ("Connecting: {0}", peerID.DisplayName);
				break;
			case MCSessionState.NotConnected:
				Console.WriteLine ("Not Connected: {0}", peerID.DisplayName);
				break;
			}
		}

		public override void DidReceiveData (MCSession session, NSData data, MCPeerID peerID)
		{
			InvokeOnMainThread (() => {
				var alert = new UIAlertView ("", data.ToString (), null, "OK");
				alert.Show ();
			});
		}

		public override void DidStartReceivingResource (MCSession session, string resourceName, MCPeerID fromPeer, NSProgress progress)
		{
		}

		public override void DidFinishReceivingResource (MCSession session, string resourceName, MCPeerID formPeer, NSUrl localUrl, NSError error)
		{
			error = null;
		}

		public override void DidReceiveStream (MCSession session, NSInputStream stream, string streamName, MCPeerID peerID)
		{
		}
	}
}

