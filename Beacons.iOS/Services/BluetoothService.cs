using System;
using Beacons.Core;
using CoreBluetooth;

namespace Beacons.iOS
{
	public class BluetoothService : IBluetoothService
	{
		#region IBluetoothService implementation
		public bool IsBlutoothEnable {
			get {
				var man = new CBCentralManager ();
				return man.State == CBCentralManagerState.PoweredOn;
			}
		}
		#endregion
	}
}

