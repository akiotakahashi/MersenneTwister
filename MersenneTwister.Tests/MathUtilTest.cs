using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MersenneTwister.Tests
{
    [TestClass]
    public class MathUtilTest
    {
        [TestMethod]
        public void MathUtil_UInt64ToDouble_c0c1()
        {
            Assert.AreEqual(0, MathUtil.UInt64ToDouble_c0c1(ulong.MinValue));
            Assert.AreEqual(1, MathUtil.UInt64ToDouble_c0c1(ulong.MaxValue));
            Assert.IsTrue(MathUtil.UInt64ToDouble_c0c1(ulong.MinValue + 1) >= MathUtil.UInt64ToDouble_c0c1(ulong.MinValue));
            Assert.IsTrue(MathUtil.UInt64ToDouble_c0c1(ulong.MaxValue - 1) <= MathUtil.UInt64ToDouble_c0c1(ulong.MaxValue));
        }

        [TestMethod]
        public void MathUtil_UInt64ToDouble_c0o1()
        {
            Assert.AreEqual(0, MathUtil.UInt64ToDouble_c0o1(ulong.MinValue));
            Assert.IsTrue(1 > MathUtil.UInt64ToDouble_c0o1(ulong.MaxValue));
            Assert.AreEqual(0, MathUtil.UInt64ToDouble_c0o1(ulong.MinValue), 1e-15);
            Assert.AreEqual(1, MathUtil.UInt64ToDouble_c0o1(ulong.MaxValue), 1e-15);
            Assert.IsTrue(MathUtil.UInt64ToDouble_c0o1(ulong.MinValue + 1) >= MathUtil.UInt64ToDouble_c0o1(ulong.MinValue));
            Assert.IsTrue(MathUtil.UInt64ToDouble_c0o1(ulong.MaxValue - 1) <= MathUtil.UInt64ToDouble_c0o1(ulong.MaxValue));
        }

        [TestMethod]
        public void MathUtil_UInt64ToDouble_o0o1()
        {
            Assert.IsTrue(0 < MathUtil.UInt64ToDouble_o0o1(ulong.MinValue));
            Assert.IsTrue(1 > MathUtil.UInt64ToDouble_o0o1(ulong.MaxValue));
            Assert.AreEqual(0, MathUtil.UInt64ToDouble_o0o1(ulong.MinValue), 1e-15);
            Assert.AreEqual(1, MathUtil.UInt64ToDouble_o0o1(ulong.MaxValue), 1e-15);
            Assert.IsTrue(MathUtil.UInt64ToDouble_o0o1(ulong.MinValue + 1) >= MathUtil.UInt64ToDouble_o0o1(ulong.MinValue));
            Assert.IsTrue(MathUtil.UInt64ToDouble_o0o1(ulong.MaxValue - 1) <= MathUtil.UInt64ToDouble_o0o1(ulong.MaxValue));
        }
        [TestMethod]
        public void MathUtil_UInt32ToDouble_c0c1()
        {
            Assert.AreEqual(0, MathUtil.UInt32ToDouble_c0c1(uint.MinValue));
            Assert.AreEqual(1, MathUtil.UInt32ToDouble_c0c1(uint.MaxValue));
            Assert.IsTrue(MathUtil.UInt32ToDouble_c0c1(uint.MinValue + 1) >= MathUtil.UInt32ToDouble_c0c1(uint.MinValue));
            Assert.IsTrue(MathUtil.UInt32ToDouble_c0c1(uint.MaxValue - 1) <= MathUtil.UInt32ToDouble_c0c1(uint.MaxValue));
        }

        [TestMethod]
        public void MathUtil_UInt32ToDouble_c0o1()
        {
            Assert.AreEqual(0, MathUtil.UInt32ToDouble_c0o1(uint.MinValue));
            Assert.IsTrue(1 > MathUtil.UInt32ToDouble_c0o1(uint.MaxValue));
            Assert.AreEqual(1, MathUtil.UInt32ToDouble_c0o1(uint.MaxValue), 1e-9);
            Assert.IsTrue(MathUtil.UInt32ToDouble_c0o1(uint.MinValue + 1) >= MathUtil.UInt32ToDouble_c0o1(uint.MinValue));
            Assert.IsTrue(MathUtil.UInt32ToDouble_c0o1(uint.MaxValue - 1) <= MathUtil.UInt32ToDouble_c0o1(uint.MaxValue));
        }

        [TestMethod]
        public void MathUtil_UInt32ToDouble_o0o1()
        {
            Assert.IsTrue(0 < MathUtil.UInt32ToDouble_o0o1(uint.MinValue));
            Assert.IsTrue(1 > MathUtil.UInt32ToDouble_o0o1(uint.MaxValue));
            Assert.AreEqual(0, MathUtil.UInt32ToDouble_o0o1(uint.MinValue), 1e-9);
            Assert.AreEqual(1, MathUtil.UInt32ToDouble_o0o1(uint.MaxValue), 1e-9);
            Assert.IsTrue(MathUtil.UInt32ToDouble_o0o1(uint.MinValue + 1) >= MathUtil.UInt32ToDouble_o0o1(uint.MinValue));
            Assert.IsTrue(MathUtil.UInt32ToDouble_o0o1(uint.MaxValue - 1) <= MathUtil.UInt32ToDouble_o0o1(uint.MaxValue));
        }

        [TestMethod]
        public void MathUtil_Next_1()
        {
            Assert.AreEqual(0, MathUtil.Next(0, uint.MinValue));
            Assert.AreEqual(0, MathUtil.Next(0, uint.MaxValue));
            Assert.AreEqual(0, MathUtil.Next(int.MaxValue, uint.MinValue));
            Assert.AreEqual(int.MaxValue - 1, MathUtil.Next(int.MaxValue, uint.MaxValue));
        }

        [TestMethod]
        public void MathUtil_Next_2()
        {
            // [0,0)
            Assert.AreEqual(0, MathUtil.Next(0, 0, uint.MinValue));
            Assert.AreEqual(0, MathUtil.Next(0, 0, uint.MaxValue));
            // [max,max)
            Assert.AreEqual(int.MinValue, MathUtil.Next(int.MinValue, int.MinValue, uint.MinValue));
            Assert.AreEqual(int.MinValue, MathUtil.Next(int.MinValue, int.MinValue, uint.MaxValue));
            // [max,max)
            Assert.AreEqual(int.MaxValue, MathUtil.Next(int.MaxValue, int.MaxValue, uint.MinValue));
            Assert.AreEqual(int.MaxValue, MathUtil.Next(int.MaxValue, int.MaxValue, uint.MaxValue));
            // [0,max)
            Assert.AreEqual(0, MathUtil.Next(0, int.MaxValue, uint.MinValue));
            Assert.AreEqual(int.MaxValue - 1, MathUtil.Next(0, int.MaxValue, uint.MaxValue));
            // [min,0)
            Assert.AreEqual(int.MinValue, MathUtil.Next(int.MinValue, 0, uint.MinValue));
            Assert.AreEqual(-1, MathUtil.Next(int.MinValue, 0, uint.MaxValue));
            // [min,max)
            Assert.AreEqual(int.MinValue, MathUtil.Next(int.MinValue, int.MaxValue, uint.MinValue));
            Assert.AreEqual(int.MaxValue - 1, MathUtil.Next(int.MinValue, int.MaxValue, uint.MaxValue));
        }
    }
}
