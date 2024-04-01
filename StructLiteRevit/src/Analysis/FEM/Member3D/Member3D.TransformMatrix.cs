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
        ///  Returns the transformation matrix for the member.
        /// </summary>
        public Matrix T()
        {
            var x1 = this.i_node.X;
            var x2 = this.j_node.X;
            var y1 = this.i_node.Y;
            var y2 = this.j_node.Y;
            var z1 = this.i_node.Z;
            var z2 = this.j_node.Z;
            var L = this.L();

            // Init direction cosines for every local axis
            var X = new Vector(new double[3]);// cos(theta), sin(theta), ??
            var Y = new Vector(new double[3]);// sin(theta), cos(theta), ??
            var Z = new Vector(new double[3]);

            // Calculate the direction cosines for the local x-axis
            // like X[1] = cos(theta),
            // where theta is angle between local x-axis and global y-axis
            X[0] = (x2 - x1) / L; //cos(theta)
            X[1] = (y2 - y1) / L; //sin(theta)
            X[2] = (z2 - z1) / L;

            // Calculate the remaining direction cosines.
            // if Vertical members
            if (Num.IsEqual(x1,x2) && Num.IsEqual(z1,z2))
            {
                // For vertical members, keep the local y-axis in the XY plane to make 2D problems easier to solve in the XY plane
                if (Num.IsFirstBiggerThanSecond(y2,y1))
                {
                    // theta = 90
                    Y[0] = -1; Y[1] = 0; Y[2] = 0;
                    Z[0] = 0;  Z[1] = 0; Z[2] = 1;
                }
                else
                {
                    // theta = -90
                    Y[0] = 1; Y[1] = 0; Y[2] = 0;
                    Z[0] = 0; Z[1] = 0; Z[2] = 1;
                }
                
            }
            // if Horizontal members
            else if (Num.IsEqual(y1, y2))
            {
                // Find a vector in the direction of the local z-axis by taking
                // the cross-product of the local x-axis and the local y-axis.
                // This vector will be perpendicular to both the local x-axis and the local y-axis.
                Y[0] = 0; Y[1] = 1; Y[2] = 0;
                Z = X.CrossProduct(Y);

                // Divide the z-vector by its magnitude to produce a unit vector of direction cosines.
                //(z[0]**2 + z[1]**2 + z[2]**2)**0.5)
                var magnitude = 1 / Math.Pow(Math.Pow(Z[0], 2) + Math.Pow(Z[1], 2) + Math.Pow(Z[2], 2), 0.5);
                Z = Z.Scale(magnitude);
            }
            // Members neither vertical or horizontal
            else
            {
                // Find the projection of x on the global XZ plane
                double[] progRaw = { x2 - x1, 0, z2 - z1 };
                var prog = new Vector(progRaw);

                // * Find a vector in the direction of the local z-axis by taking the cross-product
                //    of the local x-axis and its projection on a plane parallel to the XZ plane.
                // * This produces a vector perpendicular to both the local x-axis and its projection.
                // * This vector will always be horizontal since it's parallel to the XZ plane.
                // * The order in which the vectors are 'crossed' has been selected to ensure the y-axis always
                //    has an upward component (i.e. the top of the beam is always on top).
                if (Num.IsFirstBiggerThanSecond(y2, y1))
                {
                    Z = prog.CrossProduct(X);
                }
                else
                {
                    Z = X.CrossProduct(prog);
                }
                // Divide the z-vector by its magnitude to produce a unit vector of direction cosines.
                //(z[0]**2 + z[1]**2 + z[2]**2)**0.5)
                var magnitude = 1 / Math.Pow(Math.Pow(Z[0], 2) + Math.Pow(Z[1], 2) + Math.Pow(Z[2], 2), 0.5);
                Z = Z.Scale(magnitude);

                // Find the direction cosines for the local y-axis.
                Y = Z.CrossProduct(X);
                magnitude = 1 / Math.Pow(Math.Pow(Y[0], 2) + Math.Pow(Y[1], 2) + Math.Pow(Y[2], 2), 0.5);
                Y = Y.Scale(magnitude);
            }
            // Create the direction cosines matrix
            var dirCos = new Matrix(3);
            for (int i = 0; i < 3; i++)
            {
                dirCos[0,i] = X[i];
                dirCos[1,i] = Y[i];
                dirCos[2,i] = Z[i];
            }
            var transMatrix = new Matrix(12);
            // it is like numpy
            // transMatrix[0:3, 0:3] = dirCos
            // transMatrix[3:6, 3:6] = dirCos
            // transMatrix[6:9, 6:9] = dirCos
            // transMatrix[9:12, 9:12] = dirCos
            for (int i = 0; i < 12; i += 3)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        transMatrix[i + j, i + k] = dirCos[j, k];
                    }
                }
            }

            return transMatrix;
        }
    }
}
