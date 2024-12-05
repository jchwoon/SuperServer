using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Utils
{
    public class Utils
    {
        public static Vector3 GetDirFromRotY(float rotY)
        {
            float radians = rotY * DegreeToRadian;

            float dirX = MathF.Sin(radians);
            float dirZ = MathF.Cos(radians);

            return new Vector3(dirX, 0, dirZ);
        }

        public const float RadianToDegree = (MathF.PI / 180.0f);
        public const float DegreeToRadian = (180.0f / MathF.PI);
    }
}
