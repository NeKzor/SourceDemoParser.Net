#pragma warning disable CS0660
#pragma warning disable CS0661

namespace SourceDemoParser.Engine
{
    public struct Vector
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

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
            => Equals(vecA, vecB);
        public static bool operator !=(Vector vecA, Vector vecB)
            => !(Equals(vecA, vecB));
    }

    public struct QAngle
    {
        public float X { get; set; }    // Pitch
        public float Y { get; set; }    // Yaw
        public float Z { get; set; }    // Roll

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
            => Equals(qanA, qanB);
        public static bool operator !=(QAngle qanA, QAngle qanB)
            => !(Equals(qanA, qanB));
    }
}
