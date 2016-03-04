using Cirrious.MvvmCross.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Cirrious.CrossCore;

namespace Beacons.Core.ViewModels
{
	public class HomeViewModel 
		: MvxViewModel
	{
		string _uuid = "f7826da6-4fa2-4e98-8024-bc5b71e0893e";

		public string Uuid { 
			get { return _uuid; }
			set { 
				_uuid = value; 
				RaisePropertyChanged (() => Uuid); 
			}
		}

		string _beaconRegionId = "Discount";

		public string BeaconRegionId { 
			get { return _beaconRegionId; }
			set { 
				_beaconRegionId = value; 
				RaisePropertyChanged (() => BeaconRegionId); 
			}
		}

		string _discountInfo;

		public string DiscountInfo { 
			get { return _discountInfo; }
			set { 
				_discountInfo = value; 
				RaisePropertyChanged (() => DiscountInfo); 
			}
		}

		int _currentBeacon = -1;

		public int CurrentBeacon { 
			get { return _currentBeacon; }
			set { 
				_currentBeacon = value;
				RaisePropertyChanged (() => CurrentBeacon); 
			}
		}

		ObservableCollection<Beacon> _foundedBeacons;

		public ObservableCollection<Beacon> FoundedBeacons { 
			get { return _foundedBeacons; }
			set { 
				_foundedBeacons = value; 
				SortBeacons ();
				RaisePropertyChanged (() => FoundedBeacons); 
				UpdateCurrentBeacon ();
			}
		}

		List<Beacon> _ownBeacons;

		public List<Beacon> OwnBeacons { 
			get { return _ownBeacons; }
			set { 
				_ownBeacons = value; 
				RaisePropertyChanged (() => OwnBeacons); 
			}
		}

		public void Init ()
		{
			OwnBeacons = new List<Beacon> ();
			OwnBeacons.Add (
				new Beacon {
					Major = 52670,
					Minor = 46000,
					Name = "0aLS"
				});
			OwnBeacons.Add (
				new Beacon {
					Major = 56604,
					Minor = 46872,
					Name = "URSo"
				});
			OwnBeacons.Add (
				new Beacon {
					Major = 43085,
					Minor = 4289
				});
		}

		public void UpdateCurrentBeacon ()
		{
			if (FoundedBeacons != null && FoundedBeacons.Count > 0) {
				if (FoundedBeacons [0] == OwnBeacons [0]) {
					CurrentBeacon = 0;
					DiscountInfo = "Founded beacon 1! Discount 15%";

				} else if (FoundedBeacons [0] == OwnBeacons [1]) {
					CurrentBeacon = 1;
					DiscountInfo = "Founded beacon 2! Discount 25%";

				} else if (FoundedBeacons [0] == OwnBeacons [2]) {
					CurrentBeacon = 2;
					DiscountInfo = "Founded beacon 3! Discount 50%";
				}
			}
		}

		void SortBeacons ()
		{
			_foundedBeacons = new ObservableCollection<Beacon> (_foundedBeacons.OrderByDescending (b => b.Rssi));
		}
	}
}
