using StructLite.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Analysis
{
    public partial class Member3D
    {
        /// <summary>
        ///  Divides the element up into mathematically continuous segments along each axis.
        /// </summary>
        public void Update_segments(string combo_name = "Combo 1")
        {
            var combo = this.Model.LoadCombos[combo_name];
            var L = this.L(); 

            // Create a list of discontinuity locations
            var disconts = new List<double> { 0, L};

            foreach (var load in this.PtLoads)
            {
                if (!disconts.Contains(load.X))
                    disconts.Add(load.X);
            }
            foreach (var load in this.DistLoads)
            {
                if (!disconts.Contains(load.x1))
                    disconts.Add(load.x1);
                if (!disconts.Contains(load.x2))
                    disconts.Add(load.x2);
            }
            // Sort the list and eliminate duplicate values
            disconts.Sort();
            // Clear out old data from any previous analyses
            SegmentsZ.Clear();
            SegmentsY.Clear();
            SegmentsX.Clear();

            // Create a list of mathematically continuous segments for each direction
            for (int i = 0; i < disconts.Count - 1; i++)
            {
                // z-direction segments (bending about local z-axis)
                var newSegZ = new BeamSegZ();   // Create the new segment
                newSegZ.x1 = disconts[i];       // Segment start location
                newSegZ.x2 = disconts[i + 1];   // Segment end location
                newSegZ.EI = this.E * this.Iz;  // Segment flexural stiffness
                newSegZ.EA = this.E * this.A;   // Segment axial stiffness
                SegmentsZ.Add(newSegZ);

                // y-direction segments (bending about local y-axis)
                var newSegY = new BeamSegY();       // Create the new segment
                newSegY.x1 = disconts[i];       // Segment start location
                newSegY.x2 = disconts[i + 1];   // Segment end location
                newSegY.EI = this.E * this.Iy;  // Segment flexural stiffness
                newSegY.EA = this.E * this.A;   // Segment axial stiffness
                SegmentsY.Add(newSegY);

                // x-direction segments (for torsional moment)
                var newSegX = new BeamSegZ();       // Create the new segment
                newSegX.x1 = disconts[i];       // Segment start location
                newSegX.x2 = disconts[i + 1];   // Segment end location
                newSegX.EA = this.E * this.A;   // Segment axial stiffness
                SegmentsX.Add(newSegX);
            }

            // Member local end force vector
            var f = this.f(combo_name);
            // Member local fixed end reaction vector
            var fer = this.fer(combo_name);
            // Member local displacement vector
            var d = this.d(combo_name);

            var m1z = f[5];       // local z-axis moment at start of member
            var m2z = f[11];      // local z-axis moment at end of member
            var m1y = -f[4];      // local y-axis moment at start of member
            var m2y = -f[10];     // local y-axis moment at end of member
            var fem1z = fer[5];   // local z-axis fixed end moment at start of member
            var fem2z = fer[11];  // local z-axis fixed end moment at end of member
            var fem1y = -fer[4];  // local y-axis fixed end moment at start of member
            var fem2y = -fer[10]; // local y-axis fixed end moment at end of member
            var delta1y = d[1];   // local y displacement at start of member
            var delta2y = d[7];   // local y displacement at end of member
            var delta1z = d[2];   // local z displacement at start of member
            var delta2z = d[8];   // local z displacement at end of member

            SegmentsZ[0].delta1 = delta1y;
            SegmentsY[0].delta1 = delta1z;
            //  1/3*((m1z - fem1z)*L/(E*Iz) - (m2z - fem2z)*L/(2*E*Iz) + 3*(delta2y - delta1y)/L)
            SegmentsZ[0].theta1 = (1.0 / 3.0) * (
                (m1z - fem1z) * L / (E * Iz) 
                - (m2z - fem2z) * L / (2 * E * Iz) + 3*(delta2y - delta1y)/L
                );
            // -1/3*((m1y - fem1y)*L/(E*Iy) - (m2y - fem2y)*L/(2*E*Iy) + 3*(delta2z - delta1z)/L)
            SegmentsY[0].theta1 = -(1.0 / 3.0) * (
                (m1y - fem1y) * L / (E * Iy)
                - (m2y - fem2y) * L / (2 * E * Iy) + 3 * (delta2z - delta1z) / L
                );

            // Add the axial deflection at the start of the member
            SegmentsZ[0].delta_x1 = d[0];
            SegmentsY[0].delta_x1 = d[0];
            SegmentsX[0].delta_x1 = d[0];

            // Add loads to each segment
            for (int i = 0; i < SegmentsZ.Count; i++)
            {
                // Get the starting point of the segment
                var x = (double)SegmentsZ[i].x1;
                // Initialize the distributed loads on the segment to zero
                SegmentsZ[i].w1 = 0;
                SegmentsZ[i].w2 = 0;
                SegmentsZ[i].p1 = 0;
                SegmentsZ[i].p2 = 0;
                SegmentsY[i].w1 = 0;
                SegmentsY[i].w2 = 0;
                SegmentsY[i].p1 = 0;
                SegmentsY[i].p2 = 0;

                // Initialize the slope and displacement at the start of the segment
                // The first segment has already been initialized
                if (i > 0)
                {
                    SegmentsZ[i].theta1 = SegmentsZ[i - 1].Slope(SegmentsZ[i - 1].Length());
                    SegmentsZ[i].delta1 = SegmentsZ[i - 1].Deflection(SegmentsZ[i - 1].Length());
                    SegmentsZ[i].delta_x1 = SegmentsZ[i - 1].AxialDeflection(SegmentsZ[i - 1].Length());
                    SegmentsY[i].theta1 = SegmentsY[i - 1].Slope(SegmentsY[i - 1].Length());
                    SegmentsY[i].delta1 = SegmentsY[i - 1].Deflection(SegmentsY[i - 1].Length());
                    SegmentsY[i].delta_x1 = SegmentsY[i - 1].AxialDeflection(SegmentsY[i - 1].Length());
                }
                // Add the effects of the beam end forces to the segment
                SegmentsZ[i].P1 = f[0];
                SegmentsZ[i].V1 = f[1];
                SegmentsZ[i].M1 = f[5] - f[1] * x;
                SegmentsY[i].P1 = f[0];
                SegmentsY[i].V1 = f[2];
                SegmentsY[i].M1 = f[4] + f[2] * x;
                SegmentsX[i].T1 = f[3];

                foreach (var kvp in combo.Factors)
                {
                    var case_name = kvp.Key;
                    var factor = kvp.Value;

                    // Add effects of point loads occuring prior to this segment
                    foreach (var PtLoad in this.PtLoads)
                    {
                        if (case_name == PtLoad.CaseName && Num.IsFirstSmallerOrEqualThanSecond(PtLoad.X,x))
                        {
                            if (PtLoad.direction == Direction.Fx)
                            {
                                SegmentsZ[i].P1 += factor * PtLoad.P;
                            }
                            else if (PtLoad.direction == Direction.Fy)
                            {
                                SegmentsZ[i].V1 += factor * PtLoad.P;
                                SegmentsZ[i].M1 -= factor * PtLoad.P * (x - PtLoad.X);
                            }
                            else if (PtLoad.direction == Direction.Fz)
                            {
                                SegmentsY[i].V1 += factor * PtLoad.P;
                                SegmentsY[i].M1 += factor * PtLoad.P * (x - PtLoad.X);
                            }
                            else if (PtLoad.direction == Direction.Mx)
                            {
                                SegmentsX[i].T1 += factor * PtLoad.P;
                            }
                            else if (PtLoad.direction == Direction.My)
                            {
                                SegmentsY[i].M1 += factor * PtLoad.P;
                            }
                            else if (PtLoad.direction == Direction.Mz)
                            {
                                SegmentsZ[i].M1 += factor * PtLoad.P;
                            }
                            else if (PtLoad.direction == Direction.FX || PtLoad.direction == Direction.FY || PtLoad.direction == Direction.FZ)
                            {
                                double FX = 0;
                                double FY = 0;
                                double FZ = 0;
                                if (PtLoad.direction == Direction.FX)
                                    FX = 1;
                                else if (PtLoad.direction == Direction.FY)
                                    FY = 1;
                                else if (PtLoad.direction == Direction.FZ)
                                    FZ = 1;
                                var DirCos = new Matrix(3);
                                var T = this.T();
                                for (int row = 0; row < 3; row++)
                                {
                                    for (int col = 0; col < 3; col++)
                                    {
                                        DirCos[row, col] = T[row,col];
                                    }
                                }
                                var loadMat = new Matrix(3,1);
                                loadMat[0,0] = FX*PtLoad.P;
                                loadMat[1,0] = FY*PtLoad.P;
                                loadMat[2,0] = FZ*PtLoad.P;
                                Vector force = Vector.FromMatrix(DirCos.Multiply(loadMat));
                                SegmentsZ[i].P1 += factor * force[0];
                                SegmentsZ[i].V1 += factor * force[1];
                                SegmentsZ[i].M1 += factor * force[1] * (x - PtLoad.X);
                                SegmentsY[i].V1 += factor * force[2];
                                SegmentsY[i].M1 += factor * force[2] * (x - PtLoad.X);
                            }
                            else if (PtLoad.direction == Direction.MX || PtLoad.direction == Direction.MY || PtLoad.direction == Direction.MZ)
                            {
                                double MX = 0;
                                double MY = 0;
                                double MZ = 0;
                                if (PtLoad.direction == Direction.MX)
                                    MX = 1;
                                else if (PtLoad.direction == Direction.MY)
                                    MY = 1;
                                else if (PtLoad.direction == Direction.MZ)
                                    MZ = 1;
                                var DirCos = new Matrix(3);
                                var T = this.T();
                                for (int row = 0; row < 3; row++)
                                {
                                    for (int col = 0; col < 3; col++)
                                    {
                                        DirCos[row, col] = T[row, col];
                                    }
                                }
                                var loadMat = new Matrix(3, 1);
                                loadMat[0, 0] = MX * PtLoad.P;
                                loadMat[1, 0] = MY * PtLoad.P;
                                loadMat[2, 0] = MZ * PtLoad.P;
                                Vector force = Vector.FromMatrix(DirCos.Multiply(loadMat));
                                SegmentsX[i].T1 += factor * force[0];
                                SegmentsY[i].M1 += factor * force[1];
                                SegmentsZ[i].M1 += factor * force[2];
                            }
                        }
                    }
                    // Add distributed loads to the segment
                    foreach (var distLoad in this.DistLoads)
                    {
                        if (case_name == distLoad.CaseName)
                        {
                            // Get the parameters for the distributed load
                            var D = distLoad.direction;
                            var w1 = factor * distLoad.w1;
                            var w2 = factor * distLoad.w2;
                            var x1 = distLoad.x1;
                            var x2 = distLoad.x2;

                            // Determine if the load affects the segment
                            if (Num.IsFirstSmallerOrEqualThanSecond(x1, x))
                            {
                                if (D == Direction.Fx)
                                {
                                    //  Determine if the load is on this segment
                                    if (Num.IsFirstBiggerThanSecond(x2, x))
                                    {
                                        // Break up the load and place it on the segment
                                        // Note that 'w1' and 'w2' are really the axial loads 'p1' and 'p2' here
                                        SegmentsZ[i].p1 += (w2 - w1) / (x2 - x1) * (x - x1) + w1;
                                        SegmentsZ[i].p2 += (w2 - w1) / (x2 - x1) * (SegmentsZ[i].x2 - x1) + w1;
                                        SegmentsY[i].p1 += (w2 - w1) / (x2 - x1) * (x - x1) + w1;
                                        SegmentsY[i].p2 += (w2 - w1) / (x2 - x1) * (SegmentsY[i].x2 - x1) + w1;
                                    }
                                    // Calculate the axial force at the start of the segment
                                    SegmentsZ[i].P1 += (w1 + w2) / 2 * (x2 - x1);
                                    SegmentsY[i].P1 += (w1 + w2) / 2 * (x2 - x1);
                                }
                                else if (D == Direction.Fy)
                                {
                                    //  Determine if the load is on this segment
                                    if (Num.IsFirstBiggerThanSecond(x2, x))
                                    {
                                        // Break up the load and place it on the segment
                                        SegmentsZ[i].w1 += (w2 - w1) / (x2 - x1) * (x - x1) + w1;
                                        SegmentsZ[i].w2 += (w2 - w1) / (x2 - x1) * (SegmentsZ[i].x2 - x1) + w1;

                                        // Calculate the magnitude of the load at the start of the segment
                                        // This will be used as the 'x2' value for the load prior to the start of the segment
                                        w2 = w1 + (w2 - w1) / (x2 - x1) * (x - x1);
                                        x2 = x;
                                    }
                                    // Calculate the shear and moment at the start of the segment due to the load
                                    SegmentsZ[i].V1 += (w1 + w2) / 2 * (x2 - x1);
                                    SegmentsZ[i].M1 -= (x1 - x2) * (2 * w1 * x1 - 3 * w1 * x + w1 * x2 + w2 * x1 - 3 * w2 * x + 2 * w2 * x2) / 6;
                                }
                                else if (D == Direction.Fz)
                                {
                                    //  Determine if the load is on this segment
                                    if (Num.IsFirstBiggerThanSecond(x2, x))
                                    {
                                        // Break up the load and place it on the segment
                                        SegmentsY[i].w1 += (w2 - w1) / (x2 - x1) * (SegmentsY[i].x1 - x1) + w1;
                                        SegmentsY[i].w2 += (w2 - w1) / (x2 - x1) * (SegmentsY[i].x2 - x1) + w1;

                                        // Calculate the magnitude of the load at the start of the segment
                                        w2 = w1 + (w2 - w1) / (x2 - x1) * (x - x1);
                                        x2 = x;
                                    }
                                    // Calculate the shear and moment at the start of the segment due to the load
                                    SegmentsY[i].V1 += (w1 + w2) / 2 * (x2 - x1);
                                    SegmentsY[i].M1 += (x1 - x2) * (2 * w1 * x1 - 3 * w1 * x + w1 * x2 + w2 * x1 - 3 * w2 * x + 2 * w2 * x2) / 6;
                                }
                                else if (D == Direction.FX || D == Direction.FY || D == Direction.FZ)
                                {
                                    double FX = 0;
                                    double FY = 0;
                                    double FZ = 0;
                                    if (D == Direction.FX)
                                        FX = 1;
                                    else if (D == Direction.FY)
                                        FY = 1;
                                    else if (D == Direction.FZ)
                                        FZ = 1;
                                    var DirCos = new Matrix(3);
                                    var T = this.T();
                                    for (int row = 0; row < 3; row++)
                                    {
                                        for (int col = 0; col < 3; col++)
                                        {
                                            DirCos[row, col] = T[row, col];
                                        }
                                    }
                                    var loadMat1 = new Matrix(3, 1);
                                    loadMat1[0, 0] = FX * distLoad.w1;
                                    loadMat1[1, 0] = FY * distLoad.w1;
                                    loadMat1[2, 0] = FZ * distLoad.w1;
                                    var loadMat2 = new Matrix(3, 1);
                                    loadMat2[0, 0] = FX * distLoad.w2;
                                    loadMat2[1, 0] = FY * distLoad.w2;
                                    loadMat2[2, 0] = FZ * distLoad.w2;
                                    Vector f1 = Vector.FromMatrix(DirCos.Multiply(loadMat1));
                                    Vector f2 = Vector.FromMatrix(DirCos.Multiply(loadMat2));

                                    // Determine if the load is on this segment
                                    if (Num.IsFirstBiggerThanSecond(x2,x))
                                    {
                                        // Break up the load and place it on the segment
                                        SegmentsZ[i].p1 += (f2[0] - f1[0]) / (x2 - x1) * (x - x1) + f1[0];
                                        SegmentsZ[i].p2 += (f2[0] - f1[0]) / (x2 - x1) * (SegmentsZ[i].x2 - x1) + f1[0];
                                        SegmentsY[i].p1 += (f2[0] - f1[0]) / (x2 - x1) * (x - x1) + f1[0];
                                        SegmentsY[i].p2 += (f2[0] - f1[0]) / (x2 - x1) * (SegmentsY[i].x2 - x1) + f1[0];

                                        SegmentsZ[i].w1 += (f2[1] - f1[1]) / (x2 - x1) * (x - x1) + f1[1];
                                        SegmentsZ[i].w2 += (f2[1] - f1[1]) / (x2 - x1) * (SegmentsZ[i].x2 - x1) + f1[1];

                                        SegmentsY[i].w1 += (f2[2] - f1[2]) / (x2 - x1) * (SegmentsY[i].x1 - x1) + f1[2];
                                        SegmentsY[i].w2 += (f2[2] - f1[2]) / (x2 - x1) * (SegmentsY[i].x2 - x1) + f1[2];

                                        // Calculate the magnitude of the load at the start of the segment
                                        w2 = w1 + (w2 - w1) / (x2 - x1) * (x - x1);
                                        loadMat2 = new Matrix(3, 1);
                                        loadMat2[0, 0] = FX * distLoad.w2;
                                        loadMat2[1, 0] = FY * distLoad.w2;
                                        loadMat2[2, 0] = FZ * distLoad.w2;
                                        f2 = Vector.FromMatrix(DirCos.Multiply(loadMat2));
                                        x2 = x;
                                    }
                                    // Calculate the axial force, shear and moment at the start of the segment
                                    SegmentsZ[i].P1 += (f1[0] + f2[0]) / 2 * (x2 - x1);
                                    SegmentsY[i].P1 += (f1[0] + f2[0]) / 2 * (x2 - x1);

                                    SegmentsZ[i].V1 += (f1[1] + f2[1]) / 2 * (x2 - x1);
                                    SegmentsZ[i].M1 -= (x1 - x2) * (2 * f1[1] * x1 - 3 * f1[1] * x + f1[1] * x2 + f2[1] * x1 - 3 * f2[1] * x + 2 * f2[1] * x2) / 6;

                                    SegmentsY[i].V1 += (f1[2] + f2[2]) / 2 * (x2 - x1);
                                    SegmentsY[i].M1 += (x1 - x2) * (2 * f1[2] * x1 - 3 * f1[2] * x + f1[2] * x2 + f2[2] * x1 - 3 * f2[2] * x + 2 * f2[2] * x2) / 6;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
