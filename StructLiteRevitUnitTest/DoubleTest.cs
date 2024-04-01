using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using StructLite.Mathematics;

namespace StructLiteRevitUnitTest
{
    [TestClass]
    public class DoubleTest
    {
        [TestMethod]
        public void IsEqual()
        {
            Assert.IsTrue(Num.IsEqual(3.0000223, 3.0000228) && !Num.IsEqual(3.0000223, 3.0000238));
        }
        [TestMethod]
        public void IsFirstBiggerThanSecond()
        {
            Assert.IsTrue(Num.IsFirstBiggerThanSecond(3.000023, 3.000021) && !Num.IsFirstBiggerThanSecond(3.0000229, 3.000022));
        }
        [TestMethod]
        public void IsFirstBiggerOrEqualThanSecond()
        {
            Assert.IsTrue(Num.IsFirstBiggerOrEqualThanSecond(3.0000223, 3.0000228) && !Num.IsFirstBiggerOrEqualThanSecond(3.0000209, 3.000022));
        }
        [TestMethod]
        public void IsFirstSmallerThanSecond()
        {
            Assert.IsTrue(Num.IsFirstSmallerThanSecond(3.000024, 3.000026) && !Num.IsFirstSmallerThanSecond(3.0000223, 3.0000228));
        }
        [TestMethod]
        public void IsFirstSmallerOrEqualThanSecond()
        {
            Assert.IsTrue(Num.IsFirstSmallerOrEqualThanSecond(3.00223, 3.00228) && !Num.IsFirstSmallerOrEqualThanSecond(3.0023, 3.0021));
        }
    }
}
