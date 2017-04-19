using System.Diagnostics;

namespace SourceDemoParser_CLI.Helpers
{
	[DebuggerDisplay("{X}, {Y}, {Z}")]
	public struct Point3D
	{
		public float X;
		public float Y;
		public float Z;

		public Point3D(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public override string ToString()
			=> $"{X}, {Y}, {Z}";
	}
}