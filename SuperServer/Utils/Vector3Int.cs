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
        //public static Vector3Int operator +(Vector3Int a, Vector3Int b)
        //{
        //    return new Vector3Int(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        //}

        public static Vector3Int operator -(Vector3Int a, Vector3Int b)
        {
            return new Vector3Int(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        // 스칼라 곱셈 연산자
        //public static Vector3Int operator *(Vector3Int a, int scalar)
        //{
        //    return new Vector3Int(a.X * scalar, a.Y * scalar, a.Z * scalar);
        //}

        // 스칼라 나눗셈 연산자
        //public static Vector3Int operator /(Vector3Int a, int scalar)
        //{
        //    return new Vector3Int(a.X / scalar, a.Y / scalar, a.Z / scalar);
        //}

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
