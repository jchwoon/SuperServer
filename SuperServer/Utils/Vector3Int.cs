using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Utils
{
    public struct Vector3Int
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Vector3Int(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public static Vector3Int forward = new Vector3Int(0, 0, 1);
        public static Vector3Int forwardRight = new Vector3Int(1, 0, 1);
        public static Vector3Int right = new Vector3Int(1, 0, 0);
        public static Vector3Int backwardRight = new Vector3Int(1, 0, -1);
        public static Vector3Int backward = new Vector3Int(0, 0, -1);
        public static Vector3Int backwardLeft = new Vector3Int(-1, 0, -1);
        public static Vector3Int left = new Vector3Int(-1, 0, 0);
        public static Vector3Int forwardLeft = new Vector3Int(-1, 0, 1);

        public static Vector3Int Vector3ToVector3Int(Vector3 vector)
        {
            return new Vector3Int(
                (int)MathF.Round(vector.X),
                (int)MathF.Round(vector.Y),
                (int)MathF.Round(vector.Z)
            );
        }

        public static Vector3Int operator -(Vector3Int a, Vector3Int b)
        {
            return new Vector3Int(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }
        public static Vector3Int operator +(Vector3Int a, Vector3Int b)
        {
            return new Vector3Int(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }
        public static Vector3Int operator *(Vector3Int a, int value)
        {
            return new Vector3Int(a.X * value, a.Y * value, a.Z * value);
        }
        public static bool operator ==(Vector3Int a, Vector3Int b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }
        public static bool operator !=(Vector3Int a, Vector3Int b)
        {
            return !(a == b);
        }
        public static float Distance(Vector3Int a, Vector3Int b)
        {
            return (a - b).Magnitude();
        }
        public int MagnitudeSqr()
        {
            return X * X + Y * Y + Z * Z;
        }

        public float Magnitude()
        {
            return (float)Math.Sqrt(MagnitudeSqr());
        }
    }
}
