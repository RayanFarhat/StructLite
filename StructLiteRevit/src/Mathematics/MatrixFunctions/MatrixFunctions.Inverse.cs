﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Mathematics
{
    internal static partial class MatrixFunctions
    {
        /// <summary>
        /// Returns a value indicates whether the specified matrix is invertible. Internally uses array determinant.
        /// </summary>
        public static bool IsInvertible(double[,] input)
        {
            var rowCount = input.GetLength(0);
            var colCount = input.GetLength(1);

            if (rowCount != colCount)
                return false;

            return Determinant(input) != 0;
        }

        /// <summary>
        /// Calculates the inverse of a matrix. Returns null if non-invertible.
        /// </summary>
        public static double[,] Invert(double[,] matrix)
        {
            var rowCount = matrix.GetLength(0);
            var colCount = matrix.GetLength(1);

            if (rowCount != colCount)
                throw new InvalidOperationException("Square matrix required.");

            var newMatrix = ConcatHorizontally(matrix, CreateIdentityMatrix(rowCount));

            var result = Eliminate(newMatrix, MatrixReductionForm.ReducedRowEchelonForm, rowCount);
            if (result.Rank != colCount)
                return null;

            return result.AugmentedColumns;
        }
    }
}
