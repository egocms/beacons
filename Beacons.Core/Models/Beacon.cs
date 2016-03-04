using System;

namespace Beacons.Core
{
	public enum BeaconPriximity : long
	{
		Unknown,
		Immediate,
		Near,
		Far
	}

	public class Beacon
	{
		public BeaconPriximity Proximity { get; set; }

		public int Major { get; set; }

		public int Minor { get; set; }

		public int Rssi { get; set; }

		public string Name { get; set; }

		public static bool operator == (Beacon b1, Beacon b2)
		{
			if (ReferenceEquals (b1, null)) {
				return false;
			}
			if (ReferenceEquals (b2, null)) {
				return false;
			}
			if (b1 == null || b2 == null || b1.GetType () != b2.GetType ()) {
				return false;
			}
				
			return b1.Major.Equals (b2.Major) && b1.Minor.Equals (b2.Minor);
		}

		public static bool operator != (Beacon b1, Beacon b2)
		{
			return !(b1 == b2);
		}
	}
}

