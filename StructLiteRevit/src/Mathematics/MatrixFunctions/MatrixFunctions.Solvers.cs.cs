using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Mathematics
{
    internal static partial class MatrixFunctions
    {
        public static double[] Solve(double[,] coefficientsOrginal, double[] constantsOrginal)
        {
            // clone so the orginal does not get effected
            var coefficients = coefficientsOrginal.Clone() as double[,];
            var constants = constantsOrginal.Clone() as double[];

            int n = coefficientsOrginal.GetLength(0);
            double[] X = new double[n];

            // Gaussian elimination method
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    double factor = coefficients[j, i] / coefficients[i, i];
                    for (int k = i; k < n; k++)
                    {
                        coefficients[j, k] -= factor * coefficients[i, k];
                    }
                    constants[j] -= factor * constants[i];
                }
            }

            // Back substitution
            for (int i = n - 1; i >= 0; i--)
            {
                double sum = 0;
                for (int j = i + 1; j < n; j++)
                {
                    sum += coefficients[i, j] * X[j];
                }
                X[i] = (constants[i] - sum) / coefficients[i, i];
            }

            return X;
        }
    }
}
