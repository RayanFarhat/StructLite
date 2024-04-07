using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Mathematics
{
    public static class Converters
    {
        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        public static double DegreesToRadians(double degree)
        {
            return degree * Math.PI / 180;
        }

        /// <summary>
        /// Converts radians to degrees.
        /// </summary>
        public static double RadiansToDegrees(double radians)
        {
            return radians / Math.PI * 180;
        }
    }
}
