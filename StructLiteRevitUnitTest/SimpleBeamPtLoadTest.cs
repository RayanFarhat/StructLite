using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructLite.Analysis;
using StructLite.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLiteRevitUnitTest
{
    [TestClass]
    public class SimpleBeamPtLoadTest
    {
        private FEModel3D model;
        public SimpleBeamPtLoadTest() {
            model = new FEModel3D();
            model.AddNode(0, 0, 0, "n1");
            model.AddNode(10, 0, 0, "n2");
            model.add_material(29000.0, 11200, 0.3, 2.836e-4, null, "mat");
            model.AddMember("n1", "n2", "mat", 100, 100.0, 250, 20, "elem");
            model.def_support("n1", true, true, true, true, true, true);
            model.def_support("n2", true, true, true, true, true, true);

            model.add_member_pt_load("elem", Direction.Fy, -100, 5);

            model.Analyze();
        }
        [TestMethod]
        public void ShearTest()
        {
            var max = 50;
            var min = -50;
            Assert.IsTrue(max == model.Members["elem"].Max_Shear(Direction.Fy));
            Assert.IsTrue(min == model.Members["elem"].Min_Shear(Direction.Fy));
        }
    }
}
