using System;
using CoreBluetooth;

namespace Beacons.iOS
{
	class BTPeripheralDelegate : CBPeripheralManagerDelegate
	{
		public override void StateUpdated (CBPeripheralManager peripheral)
		{
			if (peripheral.State == CBPeripheralManagerState.PoweredOn) {
				Console.WriteLine ("powered on");
			}
		}
	}
}

