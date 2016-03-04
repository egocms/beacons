// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Beacons.iOS
{
	[Register ("HomeView")]
	partial class HomeView
	{
		[Outlet]
		UIKit.UIImageView SaleImageView { get; set; }

		[Outlet]
		UIKit.UILabel SaleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (SaleImageView != null) {
				SaleImageView.Dispose ();
				SaleImageView = null;
			}

			if (SaleLabel != null) {
				SaleLabel.Dispose ();
				SaleLabel = null;
			}
		}
	}
}
