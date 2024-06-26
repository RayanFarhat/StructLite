﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Mathematics
{
    public partial class Matrix
    {
        #region Extraction
        /// <summary>
        /// Returns a new matrix consists of the specified column range.
        /// </summary>
        public virtual Matrix ExtractColumns(int startCol, int endCol)
        {
            return new Matrix(MatrixFunctions.ExtractColumns(this.InnerMatrix, startCol, endCol));
        }
        /// <summary>
        /// Returns a new matrix consists of the specified row range.
        /// </summary>
        public virtual Matrix ExtractRows(int startRow, int endRow)
        {
            return new Matrix(MatrixFunctions.ExtractRows(this.InnerMatrix, startRow, endRow));
        }

        /// <summary>
        /// Returns a new matrix consists of the specified column indexes.
        /// </summary>
        public virtual Matrix ExtractColumns(int[] cols)
        {
            return new Matrix(MatrixFunctions.ExtractColumns(this.InnerMatrix, cols));
        }
        /// <summary>
        /// Returns a new matrix consists of the specified row indexes.
        /// </summary>
        public virtual Matrix ExtractRows(int[] rows)
        {
            return new Matrix(MatrixFunctions.ExtractRows(this.InnerMatrix, rows));
        }
        #endregion


        #region Column and Row Vectors
        /// <summary>
        /// Returns an array of column vectors of current matrix.
        /// </summary>
        public virtual Vector[] GetColumnVectors()
        {
            return MatrixFunctions.GetColumnVectors(this.InnerMatrix).Select(s => new Vector(s)).ToArray();
        }

        /// <summary>
        /// Returns an array of row vectors of current matrix.
        /// </summary>
        public virtual Vector[] GetRowVectors()
        {
            return MatrixFunctions.GetRowVectors(this.InnerMatrix).Select(s => new Vector(s)).ToArray();
        }
        #endregion
    }
}
