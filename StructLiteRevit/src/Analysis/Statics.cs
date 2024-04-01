using StructLite.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Analysis
{
    public static class Statics
    {
        /// <summary>
        /// Calculate the moment for every direction from any Point Load
        /// </summary>
        /// <param name="DistanceVector">The 3D vector that is the distance from the node and the load</param>
        /// <param name="PtLoadVector">The 3D vector That tell the direction and force of the PtLoad</param>
        /// <returns></returns>
        public static Vector MomentVectorFromCrossProduct(Vector DistanceVector, Vector PtLoadVector)
        {
            return DistanceVector.CrossProduct(PtLoadVector);
        }
        public static void MakeNodeSupport(Node3D node, string type)
        {
            switch (type)
            {
                case "ball ans socket":
                    {
                        node.support_DX = true;
                        node.support_DY = true;
                        node.support_DZ = true;
                        node.support_RX = false;
                        node.support_RY = false;
                        node.support_RZ = false;
                    }
                    break;
                case "roller":
                    {
                        node.support_DX = false;
                        node.support_DY = true;// Y axis is up and down
                        node.support_DZ = false;
                        node.support_RX = false;
                        node.support_RY = false;
                        node.support_RZ = false;
                    }
                    break;
                case "fixed":
                    {
                        node.support_DX = true;
                        node.support_DY = true;
                        node.support_DZ = true;
                        node.support_RX = true;
                        node.support_RY = true;
                        node.support_RZ = true;
                    }
                    break;
            }
        }
    }
}
