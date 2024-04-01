using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructLite.Mathematics;


namespace StructLiteRevitUnitTest
{
    [TestClass]
    public class MatrixTest
    {

        [TestMethod]
        public void InvertMatrix()
        {
            var m = new Matrix(3);
            m[0, 0] = 6;  m[0, 1] = 1;  m[0, 2] = 1;
            m[1, 0] = 4;  m[1, 1] = -2; m[1, 2] = 5;
            m[2, 0] = 2;  m[2, 1] = 8;  m[2, 2] = 7;

            var inv = new Matrix(3);
            inv[0, 0] = 0.17647059; inv[0, 1] = -0.00326797; inv[0, 2] = -0.02287582;
            inv[1, 0] = 0.05882353; inv[1, 1] = -0.13071895; inv[1, 2] = 0.08496732;
            inv[2, 0] = -0.11764706; inv[2, 1] = 0.1503268; inv[2, 2] = 0.05228758;
            Assert.IsTrue(m.Invert().Equals(inv));
        }

        [TestMethod]
        public void SolveMatrix()
        {
            var m = new Matrix(2);
            m[0, 0] = 1;
            m[0, 1] = 2;
            m[1, 0] = 3;
            m[1, 1] = 5;

            var b = new Vector(2);
            b[0] = 1;
            b[1] = 2;

            var a = m.Solve(b);

            var exepected_a = new Vector(2);
            exepected_a[0] = -1;
            exepected_a[1] = 1;
            
            Assert.IsTrue(a.Equals(exepected_a));
        }
    }
}
