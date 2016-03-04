using Android.Bluetooth;
using Beacons.Core;

namespace Beacons.Droid
{
	public class BluetoothService : IBluetoothService
	{
		#region IBluetoothService implementation
		public bool IsBlutoothEnable {
			get {
				BluetoothAdapter btAdapter = BluetoothAdapter.DefaultAdapter;
				return btAdapter.IsEnabled;
			}
		}
		#endregion

	}
}

