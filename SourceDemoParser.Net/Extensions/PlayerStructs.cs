namespace SourceDemoParser.Net.Extensions
{
	public struct PlayerPosition
	{
		public Vector3f Old;
		public Vector3f Current;
	}
	
	public struct PlayerCommand
	{
		public string Old;
		public string Current;
	}
	
	public struct Vector3f
	{
		public float X;
		public float Y;
		public float Z;
		
		public Vector3f(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}
		
		public override string ToString()
			=> $"{X}, {Y}, {Z}";
		
		public static bool Equals(Vector3f vecA, Vector3f vecB)
			=> (vecA.X == vecB.X) && (vecA.Y == vecB.Y) && (vecA.Z == vecB.Z);
	}
}