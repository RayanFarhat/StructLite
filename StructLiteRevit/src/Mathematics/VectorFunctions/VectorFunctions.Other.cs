using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Mathematics
{
    internal static partial class VectorFunctions
    {
        public static double[,] ToMatrix(double[] input)
        {
            double[,] output = new double[input.Length, 1];

            for (int i = 0; i < input.Length; i++)
                output[i, 0] = input[i];

            return output;
        }


        internal static double[] Round(double[] input, int decimals)
        {
            var length = input.Length;
            double[] output = new double[length];

            for (int i = 0; i < length; i++)
                output[i] = Math.Round(input[i], decimals);

            return output;
        }

        public static double[] ToVector(double[,] matrix)
        {
            var rowCount = matrix.GetLength(0);
            var colCount = matrix.GetLength(1);

            if (false == (rowCount == 1 || colCount == 1))
                throw new InvalidOperationException("Invalid matrix.");

            int length = rowCount == 1 ? colCount : rowCount;
            double[] output = new double[length];
            for (int i = 0; i < length; i++)
            {
                output[i] = rowCount == 1 ? matrix[0, i] : matrix[i, 0];
            }

            return output;
        }
    }
}
