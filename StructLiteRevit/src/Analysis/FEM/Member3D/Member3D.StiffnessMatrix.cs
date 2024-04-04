using StructLite.Mathematics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Analysis
{
    public partial class Member3D
    {
        /// <summary>
        ///  Returns the local stiffness matrix for the member.
        /// </summary>
        public Matrix k()
        {
            var E = this.E;
            var G = this.G;
            var Iy = this.Iy;
            var Iz = this.Iz;
            var Jx = this.Jx;
            var A = this.A;
            var L = this.L();

            //simplify the following expressions
            // Ra is the axial rigidity
            // Rt the torsional rigidity
            // Rb are bending rigities scaled by the length in various ways
            var Ra = E * A / L;
            var Rt = G * Jx / L;
            var Rby = E * Iy / L;
            var Rby2 = E * Iy / (L * L);
            var Rby3 = E * Iy / (L * L * L);
            var Rbz = E * Iz / L;
            var Rbz2 = E * Iz / (L * L);
            var Rbz3 = E * Iz / (L * L * L);

            double[,] kRaw = new double[,]
                {
                    {  Ra,        0,        0,   0,       0,       0, -Ra,        0,        0,   0,      0,       0 },//DXi

                    {   0,  12*Rbz3,        0,   0,       0,  6*Rbz2,   0, -12*Rbz3,        0,   0,      0,  6*Rbz2 },//DYi

                    {   0,        0,  12*Rby3,   0, -6*Rby2,       0,   0,        0, -12*Rby3,   0,  -6*Rby2,       0 },//DZi

                    {   0,        0,        0,  Rt,       0,       0,   0,        0,        0, -Rt,      0,       0 },//RXi

                    {   0,        0,  -6*Rby2,   0,   4*Rby,       0,   0,        0,   6*Rby2,   0,  2*Rby,       0 },//RYi

                    {   0,   6*Rbz2,        0,   0,       0,   4*Rbz,   0,  -6*Rbz2,        0,   0,      0,   2*Rbz },//RZi

                    { -Ra,        0,        0,   0,       0,       0,  Ra,        0,        0,   0,      0,       0 },//DXj 

                    {   0, -12*Rbz3,        0,   0,       0, -6*Rbz2,   0,  12*Rbz3,        0,   0,      0, -6*Rbz2 },//DYj

                    {   0,        0, -12*Rby3,   0,  6*Rby2,       0,   0,        0,  12*Rby3,   0, 6*Rby2,       0 },//DZi

                    {   0,        0,        0, -Rt,       0,       0,   0,        0,        0,  Rt,      0,       0 },//RXj

                    {   0,        0,  -6*Rby2,   0,   2*Rby,       0,   0,        0,   6*Rby2,   0,  4*Rby,       0 },//Yj

                    {   0,   6*Rbz2,        0,   0,       0,   2*Rbz,   0,  -6*Rbz2,        0,   0,      0,   4*Rbz }//RZj

                };

            var k = new Matrix(kRaw);
            return k;
        }

        /// <summary>
        ///  Returns the global elastic stiffness matrix for the member.
        /// </summary>
        public Matrix K()
        {
            //T^-1 * k * T
            var T = this.T();
            return T.Invert().Multiply(this.k()).Multiply(T);
        }
    }
}
