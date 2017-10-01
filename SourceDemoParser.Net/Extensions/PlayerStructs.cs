#pragma warning disable CS0660
#pragma warning disable CS0661

namespace SourceDemoParser.Extensions
{
	public struct PlayerPosition
	{
		public Vector Old;
		public Vector Current;
	}

	public struct PlayerCommand
	{
		public string Old;
		public string Current;
	}

	public class Vector
	{
		public float X { get; set; }
		public float Y { get; set; }
		public float Z { get; set; }

		public Vector()
		{
		}
		public Vector(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public override string ToString()
			=> $"{X}, {Y}, {Z}";

		public static bool Equals(Vector vecA, Vector vecB)
			=> (vecA.X == vecB.X) && (vecA.Y == vecB.Y) && (vecA.Z == vecB.Z);
		public static bool operator ==(Vector vecA, Vector vecB)
			=> (Equals(vecA, vecB));
		public static bool operator !=(Vector vecA, Vector vecB)
			=> !(Equals(vecA, vecB));

		public byte[] GetBytes()
		{
			var bytes = new byte[0];
			X.GetBytes().AppendTo(ref bytes);
			Y.GetBytes().AppendTo(ref bytes);
			Z.GetBytes().AppendTo(ref bytes);
			return bytes;
		}
	}

	public class QAngle
	{
		public float X { get; set; }
		public float Y { get; set; }
		public float Z { get; set; }

		public QAngle()
		{
		}
		public QAngle(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public override string ToString()
			=> $"{X}, {Y}, {Z}";

		public static bool Equals(QAngle qanA, QAngle qanB)
			=> (qanA.X == qanB.X) && (qanA.Y == qanB.Y) && (qanA.Z == qanB.Z);
		public static bool operator ==(QAngle qanA, QAngle qanB)
			=> (Equals(qanA, qanB));
		public static bool operator !=(QAngle qanA, QAngle qanB)
			=> !(Equals(qanA, qanB));

		public byte[] GetBytes()
		{
			var bytes = new byte[0];
			X.GetBytes().AppendTo(ref bytes);
			Y.GetBytes().AppendTo(ref bytes);
			Z.GetBytes().AppendTo(ref bytes);
			return bytes;
		}
	}
}