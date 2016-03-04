using System.Collections.ObjectModel;
using System.Linq;
using Beacons.Core;
using Beacons.Core.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using CoreLocation;
using Foundation;
using MultipeerConnectivity;
using ObjCRuntime;
using UIKit;

namespace Beacons.iOS
{
	public partial class HomeView : MvxViewController
	{
		CLLocationManager _locationMgr;
		CLBeaconRegion _beaconRegion;
		MCSession _session;
		MCPeerID _peer;
		MCAdvertiserAssistant _assistant;
		MySessionDelegate _sessionDelegate = new MySessionDelegate ();
		NSDictionary _dict = new NSDictionary ();
		static readonly string _serviceType = "DiscountService";

		public new HomeViewModel ViewModel {
			get { return base.ViewModel as HomeViewModel; }
			set { base.ViewModel = value; }
		}

		public ObservableCollection<Beacon> FoundedBeacons { get; set; }
		public string Uuid { get; set; }
		public string BeaconRegionId { get; set; }

		int _currentBeacon;

		public int CurrentBeacon { 
			get { return _currentBeacon; }
			set { 
				_currentBeacon = value;

				switch (ViewModel.CurrentBeacon) {
				case 0:
					UpdateDisplay ("15");
					break;
				case 1:
					UpdateDisplay ("25");
					break;
				case 2:
					UpdateDisplay ("50");
					break;
				}
			}
		}

		public HomeView () : base ("HomeView", null)
		{

		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			SetupStyles ();

			var set = this.CreateBindingSet<HomeView, HomeViewModel> ();
			set.Bind (SaleLabel).To (vm => vm.DiscountInfo);
			set.Bind (this).For (p => p.CurrentBeacon).To (vm => vm.CurrentBeacon);
			set.Bind (this).For (p => p.Uuid).To (vm => vm.Uuid);
			set.Bind (this).For (p => p.BeaconRegionId).To (vm => vm.BeaconRegionId);
			set.Apply ();

			StartMultipeerAdvertiser ();
			SetupBeaconRegion ();
			SetupLocationManager ();
		}
			
		void SetupStyles ()
		{
			// ios7 layout
			if (RespondsToSelector (new Selector ("edgesForExtendedLayout"))) {
				EdgesForExtendedLayout = UIRectEdge.None;
			}
			SaleImageView.Image = UIImage.FromBundle ("sale");
			SaleImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
		}

		void StartMultipeerAdvertiser ()
		{
			_peer = new MCPeerID ("Player1");
			_session = new MCSession (_peer);
			_session.Delegate = _sessionDelegate;
			_assistant = new MCAdvertiserAssistant (_serviceType, _dict, _session); 
			_assistant.Start ();
		}

		void SetupBeaconRegion ()
		{
			_beaconRegion = new CLBeaconRegion (new NSUuid (ViewModel.Uuid), ViewModel.BeaconRegionId);
			_beaconRegion.NotifyEntryStateOnDisplay = true;
			_beaconRegion.NotifyOnEntry = true;
			_beaconRegion.NotifyOnExit = true;
		}

		void SetupLocationManager ()
		{
			_locationMgr = new CLLocationManager ();
			_locationMgr.RequestWhenInUseAuthorization ();
			_locationMgr.RegionEntered += (object sender, CLRegionEventArgs e) =>  {
				if (e.Region.Identifier == ViewModel.BeaconRegionId) {
					var notification = new UILocalNotification () {
						AlertBody = "There's a sale hiding nearby!"
					};
					UIApplication.SharedApplication.PresentLocalNotificationNow (notification);
				}
			};
			_locationMgr.DidRangeBeacons += DidRangeBeacons;
			_locationMgr.StartMonitoring (_beaconRegion);
			_locationMgr.StartRangingBeacons (_beaconRegion);
		}

		void DidRangeBeacons (object sender, CLRegionBeaconsRangedEventArgs e)
		{
			FoundedBeacons = new ObservableCollection<Beacon> (e.Beacons.Select<CLBeacon, Beacon> (b => new Beacon {
				Major = b.Major.Int32Value,
				Minor = b.Minor.Int32Value,
				Proximity = (BeaconPriximity)b.Proximity,
				Rssi = (int)b.Rssi
			}));
			ViewModel.FoundedBeacons = FoundedBeacons;
		}
			
		public void UpdateDisplay (string filename)
		{
			InvokeOnMainThread (() => {
				SaleImageView.Image = UIImage.FromBundle (filename);
			});
		}
	}
}


