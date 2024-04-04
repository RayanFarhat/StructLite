using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Mathematics
{
    public static class Num
    {
        public static bool IsFirstBiggerThanSecond(double d1, double d2)
        {
            if (Math.Abs(d1 - d2) < 0.000001)
                return false;
            if (d1 > d2)
                return true;
            return false;
        }
        public static bool IsFirstSmallerThanSecond(double d1, double d2)
        {
            if (Math.Abs(d1 - d2) < 0.000001)
                return false;
            if (d1 < d2)
                return true;
            return false;
        }
        public static bool IsFirstBiggerOrEqualThanSecond(double d1, double d2)
        {
            if (d1 > d2)
                return true;
            if (Math.Abs(d1 - d2) < 0.000001)
                return true;
            return false;
        }
        public static bool IsFirstSmallerOrEqualThanSecond(double d1, double d2)
        {
            if (d1 < d2)
                return true;
            if (Math.Abs(d1 - d2) < 0.000001)
                return true;
            return false;
        }
        public static bool IsEqual(double d1, double d2)
        {
            if (Math.Abs(d1 - d2) < 0.000001)
                return true;
            return false;
        }
        public static double[] Linspace(double start, double end, int num)
        {
            if (num < 2)
                throw new ArgumentException("Number of samples must be at least 2.");

            double[] linspaceArray = new double[num];
            double step = (end - start) / (num - 1);

            for (int i = 0; i < num; i++)
            {
                linspaceArray[i] = start + step * i;
            }

            return linspaceArray;
        }
    }
}
