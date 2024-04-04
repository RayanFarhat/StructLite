using StructLite.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Analysis
{
    internal static class FixedEndsReactions
    {
        /// <summary>
        ///   Returns the fixed end reaction vector for a point load.
        /// </summary>
        ///   <param name="P">The magnitude of the point load</param>
        ///   <param name="x">The location of the point load relative to the start of the member</param>
        ///   <param name="L">The length of the member</param>
        ///   <param name="D">The direction of the point load. Must be one of FY or FZ</param>
        public static Vector FER_PtLoad(double P, double x, double L, Direction D)
        {
            var b = L - x;
            var FER = new Vector(12);
            if (D == Direction.FY || D == Direction.Fy)
            {
                FER[1] = -P * Math.Pow(b, 2) * (L + 2 * x) / Math.Pow(L, 3);
                FER[5] = -P * x * Math.Pow(b, 2) / Math.Pow(L, 2);
                FER[7] = -P * Math.Pow(x, 2) * (L + 2 * b) / Math.Pow(L, 3);
                FER[11] = P * b * Math.Pow(x, 2) / Math.Pow(L, 2);
            }
            else if(D == Direction.FZ || D == Direction.Fz)
            {
                FER[2] = -P * Math.Pow(b, 2) * (L + 2 * x) / Math.Pow(L, 3);
                FER[4] = P * x * Math.Pow(b, 2) / Math.Pow(L, 2);
                FER[8] = -P * Math.Pow(x, 2) * (L + 2 * b) / Math.Pow(L, 3);
                FER[10] = -P * b * Math.Pow(x, 2) / Math.Pow(L, 2);
            }
            return FER;
        }
        /// <summary>
        ///   Returns the fixed end reaction vector for a concentrated moment.
        /// </summary>
        ///   <param name="M">The magnitude of the moment</param>
        ///   <param name="x">The location of the moment relative to the start of the member</param>
        ///   <param name="L">The length of the member</param>
        ///   <param name="D">The direction of the point load. Must be one of MY or MZ</param>
        public static Vector FER_Moment(double M, double x, double L, Direction D)
        {
            var b = L - x;
            var FER = new Vector(12);
            if (D == Direction.MZ || D == Direction.Mz)
            {
                FER[1] = 6 * M * x * b / Math.Pow(L, 3);
                FER[5] = M * b * (2*x - b)/Math.Pow(L, 2);
                FER[7] = -6 * M * x * b / Math.Pow(L, 3);
                FER[11] = M * x * (2 * b - x) / Math.Pow(L, 2);
            }
            else if (D == Direction.MY || D == Direction.My)
            {
                FER[2] = -6 * M * x * b / Math.Pow(L, 3);
                FER[4] = M * b * (2 * x - b) / Math.Pow(L, 2);
                FER[8] = 6 * M * x * b / Math.Pow(L, 3);
                FER[10] = M * x * (2 * b - x) / Math.Pow(L, 2);
            }
            return FER;
        }

        /// <summary>
        ///   Returns the fixed end reaction vector for an axial point load(Direction FX).
        /// </summary>
        ///   <param name="P">The magnitude of the point load</param>
        ///   <param name="x">The location of the point load relative to the start of the member</param>
        ///   <param name="L">The length of the member</param>
        public static Vector FER_AxialPtLoad(double P, double x, double L)
        {
            var b = L - x;
            var FER = new Vector(12);
            FER[0] = -P * b / L;
            FER[6] = -P * x / L;
            return FER;
        }
        /// <summary>
        ///   Returns the fixed end reaction vector for a distributed axial load(Direction FX).
        /// </summary>
        ///   <param name="x1">The start place of the load relative to the start of the member.</param>
        ///   <param name="x2">The end place of the load relative to the start of the member.</param>
        ///   <param name="w1">The start numeric value (magnitude) of the load from x1</param>
        ///   <param name="w2">The end numeric value (magnitude) of the load to x2.</param>
        ///   <param name="L">The length of the member</param>
        public static Vector FER_AxialLinLoad(double x1, double x2, double w1, double w2, double L)
        {
            var FER = new Vector(12);
            var diff = 1 / (6 * L) * (x1 - x2);
            var a = 2 * w1 * x1;
            var b = 2 * w2 * x2;
            var c = w2 * x1;
            var d = w1 * x2;
            FER[0] = diff*(3*L*w1 + 3*L*w2 - a - b - c -d);
            FER[6] = diff*(a+b+c+d);
            return FER;
        }
        /// <summary>
        ///   Returns the fixed end reaction vector for a concentrated torque.
        /// </summary>
        ///   <param name="T">The magnitude of the torque</param>
        ///   <param name="x">The location of the torque relative to the start of the member</param>
        ///   <param name="L">The length of the member</param>
        public static Vector FER_Torque(double T, double x, double L)
        {
            var b = L - x;
            var FER = new Vector(12);
            FER[3] = -T * b / L;
            FER[9] = -T * x / L;
            return FER;
        }
        /// <summary>
        ///   eturns the fixed end reaction vector for a linear distributed load.
        /// </summary>
        ///   <param name="x1">The start place of the load relative to the start of the member.</param>
        ///   <param name="x2">The end place of the load relative to the start of the member.</param>
        ///   <param name="w1">The start numeric value (magnitude) of the load from x1</param>
        ///   <param name="w2">The end numeric value (magnitude) of the load to x2.</param>
        ///   <param name="L">The length of the member</param>
        ///   <param name="D">The direction of the point load. Must be one of FY or FZ</param>

        public static Vector FER_LinLoad(double x1, double x2, double w1, double w2, double L, Direction D)
        {
            var FER = new Vector(12);

            var diff = x1 - x2;
            //(10*L**3*w1 + 10*L**3*w2 - 15*L*w1*x1**2 - 10*L*w1*x1*x2 - 5*L*w1*x2**2 - 5*L*w2*x1**2 - 10*L*w2*x1*x2 - 15*L*w2*x2**2 + 8*w1*x1**3 + 6*w1*x1**2*x2 +
            //4*w1*x1*x2**2 + 2*w1*x2**3 + 2*w2*x1**3 + 4*w2*x1**2*x2 + 6*w2*x1*x2**2 + 8*w2*x2**3)/(20*L**3))
            var a = (10 * Math.Pow(L, 3) * w1 + 10 * Math.Pow(L, 3) * w2 - 15 * L * w1 * Math.Pow(x1, 2) - 10 * L * w1 * x1 * x2 - 5 * L * w1 * Math.Pow(x2, 2)
                    - 5 * L * w2 * Math.Pow(x1, 2) - 10 * L * w2 * x1 * x2 - 15 * L * w2 * Math.Pow(x2, 2) + 8 * w1 * Math.Pow(x1, 3) + 6 * w1 * Math.Pow(x1, 2) * x2
                    + 4 * w1 * x1 * Math.Pow(x2, 2) + 2 * w1 * Math.Pow(x2, 3) + 2 * w2 * Math.Pow(x1, 3) + 4 * w2 * Math.Pow(x1, 2) * x2 + 6 * w2 * x1 * Math.Pow(x2, 2)
                    + 8 * w2 * Math.Pow(x2, 3)) / (20 * Math.Pow(L, 3));

            var b = (20 * Math.Pow(L, 2) * w1 * x1 + 10 * Math.Pow(L, 2) * w1 * x2 + 10 * Math.Pow(L, 2) * w2 * x1 + 20 * Math.Pow(L, 2) * w2 * x2 - 30 * L * w1 * Math.Pow(x1, 2)
                    - 20 * L * w1 * x1 * x2 - 10 * L * w1 * Math.Pow(x2, 2) - 10 * L * w2 * Math.Pow(x1, 2) - 20 * L * w2 * x1 * x2 - 30 * L * w2 * Math.Pow(x2, 2) + 12 * w1 * Math.Pow(x1, 3)
                    + 9 * w1 * Math.Pow(x1, 2) * x2 + 6 * w1 * x1 * Math.Pow(x2, 2) + 3 * w1 * Math.Pow(x2, 3) + 3 * w2 * Math.Pow(x1, 3) + 6 * w2 * Math.Pow(x1, 2) * x2
                    + 9 * w2 * x1 * Math.Pow(x2, 2) + 12 * w2 * Math.Pow(x2, 3)) / (60 * Math.Pow(L, 2));

            var c = (-15 * L * w1 * Math.Pow(x1, 2) - 10 * L * w1 * x1 * x2 - 5 * L * w1 * Math.Pow(x2, 2) - 5 * L * w2 * Math.Pow(x1, 2) - 10 * L * w2 * x1 * x2 - 15 * L * w2 * Math.Pow(x2, 2)
                    + 8 * w1 * Math.Pow(x1, 3) + 6 * w1 * Math.Pow(x1, 2) * x2 + 4 * w1 * x1 * Math.Pow(x2, 2) + 2 * w1 * Math.Pow(x2, 3) + 2 * w2 * Math.Pow(x1, 3)
                    + 4 * w2 * Math.Pow(x1, 2) * x2 + 6 * w2 * x1 * Math.Pow(x2, 2) + 8 * w2 * Math.Pow(x2, 3)) / (20 * Math.Pow(L, 3));

            var d = (-15 * L * w1 * Math.Pow(x1, 2) - 10 * L * w1 * x1 * x2 - 5 * L * w1 * Math.Pow(x2, 2) - 5 * L * w2 * Math.Pow(x1, 2) - 10 * L * w2 * x1 * x2 - 15 * L * w2 * Math.Pow(x2, 2)
                    + 12 * w1 * Math.Pow(x1, 3) + 9 * w1 * Math.Pow(x1, 2) * x2 + 6 * w1 * x1 * Math.Pow(x2, 2) + 3 * w1 * Math.Pow(x2, 3) + 3 * w2 * Math.Pow(x1, 3)
                    + 6 * w2 * Math.Pow(x1, 2) * x2 + 9 * w2 * x1 * Math.Pow(x2, 2) + 12 * w2 * Math.Pow(x2, 3)) / (60 * Math.Pow(L, 2));

            if (D == Direction.FY || D == Direction.Fy)
            {
                FER[1] = diff * a;
                FER[5] = diff * b;
                FER[7] = -diff * c;
                FER[11] = diff * d;
            }
            else if (D == Direction.FZ || D == Direction.Fz)
            {
                FER[2] = diff * a;
                FER[4] = -diff * b;
                FER[8] = -diff * c;
                FER[10] = -diff * d;
            }
            return FER;
        }
    }
}
