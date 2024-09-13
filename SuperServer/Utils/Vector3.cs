using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Utils
{

    public struct Vector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float MagnitudeSqr()
        {
            return X * X + Y * Y + Z * Z;
        }

        public float Magnitude()
        {
            return (float)Math.Sqrt(MagnitudeSqr());
        }

        public Vector3 Normalize()
        {
            float magnitude = Magnitude();
            if (magnitude > 0)
            {
                return new Vector3(X / magnitude, Y / magnitude, Z / magnitude);
            }
            return new Vector3(0, 0, 0);
        }
    }
}
