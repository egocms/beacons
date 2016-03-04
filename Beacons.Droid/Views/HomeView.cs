using System;
using System.Collections.ObjectModel;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Beacons.Core;
using Beacons.Core.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Droid.Views;
using RadiusNetworks.IBeaconAndroid;
using Color = Android.Graphics.Color;

namespace Beacons.Droid.Views
{
	[Activity]
	public class HomeView : MvxActivity, IBeaconConsumer
	{
		bool _paused;
		View _view;
		IBeaconManager _iBeaconManager;
		MonitorNotifier _monitorNotifier;
		RangeNotifier _rangeNotifier;
		Region _monitoringRegion;
		Region _rangingRegion;
		TextView _text;
		ImageView _saleImage;

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

				switch (_currentBeacon) {
				case 0:
					UpdateDisplay (Resource.Drawable.sale15);
					break;
				case 1:
					UpdateDisplay (Resource.Drawable.sale25);
					break;
				case 2:
					UpdateDisplay (Resource.Drawable.sale50);
					break;
				}
			}
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.FirstView);

			_view = FindViewById<RelativeLayout> (Resource.Id.findTheSaleView);
			_text = FindViewById<TextView> (Resource.Id.saleStatusLabel);
			_saleImage = FindViewById<ImageView> (Resource.Id.sale_image);

			var set = this.CreateBindingSet<HomeView, HomeViewModel> ();
			set.Bind (_text).To (vm => vm.DiscountInfo);
			set.Bind (this).For (p => p.Uuid).To (vm => vm.Uuid);
			set.Bind (this).For (p => p.BeaconRegionId).To (vm => vm.BeaconRegionId);
			set.Bind (this).For (p => p.CurrentBeacon).To (vm => vm.CurrentBeacon);
			set.Apply ();

			Init ();
		}

		void Init ()
		{
			_iBeaconManager = IBeaconManager.GetInstanceForApplication (ApplicationContext);
			_monitorNotifier = new MonitorNotifier ();
			_rangeNotifier = new RangeNotifier ();
			_monitoringRegion = new Region (BeaconRegionId, Uuid, null, null);
			_rangingRegion = new Region (BeaconRegionId, Uuid, null, null);
			
			_iBeaconManager.Bind (this);
			
			_monitorNotifier.EnterRegionComplete += EnteredRegion;
			_monitorNotifier.ExitRegionComplete += ExitedRegion;
			
			_rangeNotifier.DidRangeBeaconsInRegionComplete += RangingBeaconsInRegion;
		}

		protected override void OnResume ()
		{
			base.OnResume ();
			_paused = false;
		}

		protected override void OnPause ()
		{
			base.OnPause ();
			_paused = true;
		}

		void EnteredRegion (object sender, MonitorEventArgs e)
		{
			if (_paused) {
				ShowNotification ();
			}
		}

		void ExitedRegion (object sender, MonitorEventArgs e)
		{
		}

		void RangingBeaconsInRegion (object sender, RangeEventArgs e)
		{
			FoundedBeacons = new ObservableCollection<Beacon> (e.Beacons.Select<IBeacon, Beacon> (b => new Beacon {
				Major = b.Major,
				Minor = b.Minor,
				Proximity = (BeaconPriximity)b.Proximity,
				Rssi = (int)b.Rssi
			}));
			ViewModel.FoundedBeacons = FoundedBeacons;
		}

		#region IBeaconConsumer impl

		public void OnIBeaconServiceConnect ()
		{
			_iBeaconManager.SetMonitorNotifier (_monitorNotifier);
			_iBeaconManager.SetRangeNotifier (_rangeNotifier);

			_iBeaconManager.StartMonitoringBeaconsInRegion (_monitoringRegion);
			_iBeaconManager.StartRangingBeaconsInRegion (_rangingRegion);
		}

		#endregion

		private void UpdateDisplay (int drawable)
		{
			RunOnUiThread (() => _saleImage.SetImageDrawable (Resources.GetDrawable (drawable)));
		}

		private void ShowNotification ()
		{
			var resultIntent = new Intent (this, typeof(HomeView));
			resultIntent.AddFlags (ActivityFlags.ReorderToFront);
			var pendingIntent = PendingIntent.GetActivity (this, 0, resultIntent, PendingIntentFlags.UpdateCurrent);
			var notificationId = Resource.String.sale_notification;

			var builder = new Notification.Builder (this)
				.SetSmallIcon (Resource.Mipmap.Icon)
				.SetContentTitle (this.GetText (Resource.String.app_label))
				.SetContentText (this.GetText (Resource.String.sale_notification))
				.SetContentIntent (pendingIntent)
				.SetAutoCancel (true);

			var notification = builder.Build ();

			var notificationManager = (NotificationManager)GetSystemService (NotificationService);
			notificationManager.Notify (notificationId, notification);
		}

		protected override void OnDestroy ()
		{
			base.OnDestroy ();

			_monitorNotifier.EnterRegionComplete -= EnteredRegion;
			_monitorNotifier.ExitRegionComplete -= ExitedRegion;

			_rangeNotifier.DidRangeBeaconsInRegionComplete -= RangingBeaconsInRegion;

			_iBeaconManager.StopMonitoringBeaconsInRegion (_monitoringRegion);
			_iBeaconManager.StopRangingBeaconsInRegion (_rangingRegion);
			_iBeaconManager.UnBind (this);
		}
	}
}