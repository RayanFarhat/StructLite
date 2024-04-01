using Autodesk.Revit.UI.Selection;
using StructLite.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Analysis
{
    public partial class Member3D
    {
        /// <summary>
        ///   Returns the member's elastic local internal end force vector for the given load combination.
        /// </summary>
        public Vector f(string combo_name = "Combo 1")
        {
            var f = k().Multiply(d(combo_name));
            f = Vector.FromMatrix(f);
            return f.Add(fer(combo_name));
        }
        /// <summary>
        ///   Returns the member's global internal end force vector for the given load combination.
        /// </summary>
        public Vector F(string combo_name = "Combo 1")
        {
            //T^-1 * f
            var T_inv = this.T().Invert();
            return T_inv.Multiply(f(combo_name));
        }
        /// <summary>
        ///    Returns the member's local fixed end reaction vector, ignoring the effects of end releases.
        /// </summary>
        public Vector fer(string combo_name = "Combo 1")
        {
            var FER = new Vector(12);

            var combo = this.Model.LoadCombos[combo_name];
            //Loop through each load case and factor in the load combination
            foreach (var kvp in combo.Factors)
            {
                var case_name = kvp.Key;
                var factor = kvp.Value;
                // Sum the fixed end reactions for the point loads & moments
                foreach (var ptLoad in this.PtLoads)
                {
                    // Check if the current point load corresponds to the current load case
                    if (ptLoad.CaseName == case_name)
                    {
                        if (ptLoad.direction == Direction.Fx)
                            FER = FER.Add(FixedEndsReactions.FER_AxialPtLoad(factor*ptLoad.P,ptLoad.X,L()));
                        else if (ptLoad.direction == Direction.Fy || ptLoad.direction == Direction.Fz)
                            FER = FER.Add(FixedEndsReactions.FER_PtLoad(factor * ptLoad.P, ptLoad.X, L(), ptLoad.direction));
                        else if (ptLoad.direction == Direction.Mx)
                            FER = FER.Add(FixedEndsReactions.FER_Torque(factor * ptLoad.P, ptLoad.X, L()));
                        else if (ptLoad.direction == Direction.My || ptLoad.direction == Direction.Mz)
                            FER = FER.Add(FixedEndsReactions.FER_Moment(factor * ptLoad.P, ptLoad.X, L(), ptLoad.direction));
                        else if (ptLoad.direction == Direction.FX || ptLoad.direction == Direction.FY || ptLoad.direction == Direction.FZ)
                        {
                            double FX = 0;
                            double FY = 0;
                            double FZ = 0;
                            if (ptLoad.direction == Direction.FX)
                                FX = 1;
                            else if (ptLoad.direction == Direction.FY)
                                FY = 1;
                            else if (ptLoad.direction == Direction.FZ)
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
                            var loadMat = new Matrix(3, 1);
                            loadMat[0, 0] = FX * ptLoad.P;
                            loadMat[1, 0] = FY * ptLoad.P;
                            loadMat[2, 0] = FZ * ptLoad.P;
                            Vector force = Vector.FromMatrix(DirCos.Multiply(loadMat));
                            FER = FER.Add(FixedEndsReactions.FER_AxialPtLoad(factor * force[0], ptLoad.X, L()));
                            FER = FER.Add(FixedEndsReactions.FER_PtLoad(factor * force[1], ptLoad.X, L(), Direction.FY));
                            FER = FER.Add(FixedEndsReactions.FER_PtLoad(factor * force[2], ptLoad.X, L(), Direction.FZ));
                        }
                        else if (ptLoad.direction == Direction.MX || ptLoad.direction == Direction.MY || ptLoad.direction == Direction.MZ)
                        {
                            double MX = 0;  
                            double MY = 0;
                            double MZ = 0;
                            if (ptLoad.direction == Direction.MX)
                                MX = 1;
                            else if (ptLoad.direction == Direction.MY)
                                MY = 1;
                            else if (ptLoad.direction == Direction.MZ)
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
                            loadMat[0, 0] = MX * ptLoad.P;
                            loadMat[1, 0] = MY * ptLoad.P;
                            loadMat[2, 0] = MZ * ptLoad.P;
                            Vector force = Vector.FromMatrix(DirCos.Multiply(loadMat));
                            FER = FER.Add(FixedEndsReactions.FER_Torque(factor * force[0], ptLoad.X, L()));
                            FER = FER.Add(FixedEndsReactions.FER_Moment(factor * force[1], ptLoad.X, L(), Direction.MY));
                            FER = FER.Add(FixedEndsReactions.FER_Moment(factor * force[2], ptLoad.X, L(), Direction.MZ));
                        }
                    }
                }
                // Sum the fixed end reactions for the distributed loads
                foreach (var distLoad in this.DistLoads)
                {
                    // Check if the current distributed load corresponds to the current load case
                    if (distLoad.CaseName == case_name)
                    {
                        if (distLoad.direction == Direction.Fx)
                            FER = FER.Add(FixedEndsReactions.FER_AxialLinLoad(distLoad.x1,distLoad.x2,distLoad.w1,distLoad.w2,L()));
                        else if (distLoad.direction == Direction.Fy || distLoad.direction == Direction.Fz)
                            FER = FER.Add(FixedEndsReactions.FER_LinLoad(distLoad.x1, distLoad.x2, distLoad.w1, distLoad.w2, L(), distLoad.direction));
                        else if (distLoad.direction == Direction.FX || distLoad.direction == Direction.FY || distLoad.direction == Direction.FZ)
                        {
                            double FX = 0;
                            double FY = 0;
                            double FZ = 0;
                            if (distLoad.direction == Direction.FX)
                                FX = 1;
                            else if (distLoad.direction == Direction.FY)
                                FY = 1;
                            else if (distLoad.direction == Direction.FZ)
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
                            FER = FER.Add(FixedEndsReactions.FER_AxialLinLoad(distLoad.x1, distLoad.x2, factor * f1[0], factor * f2[0], L()));
                            FER = FER.Add(FixedEndsReactions.FER_LinLoad(distLoad.x1, distLoad.x2, factor * f1[1], factor * f2[1], L(), Direction.FY));
                            FER = FER.Add(FixedEndsReactions.FER_LinLoad(distLoad.x1, distLoad.x2, factor * f1[2], factor * f2[2], L(), Direction.FZ));
                        }
                    }
                }
            }
            //Return the fixed end reaction vector, uncondensed
            return FER;
        }
        /// <summary>
        ///     Returns the global fixed end reaction vector.
        /// </summary>
        public Vector FER(string combo_name = "Combo 1")
        {
            return T().Invert().Multiply(fer(combo_name));
        }
    }
}
