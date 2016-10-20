using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MersenneTwister.Tests
{
    [TestClass]
    public class BitScannerTest
    {
        private readonly Random rng = DsfmtRandom.Create();

        private static void Test(uint value)
        {
            Assert.AreEqual(1UL << (CalcReverse(value) + 1), (ulong)BitScanner.Mask(value) + 1);
        }

        private static int CalcForward(ulong value)
        {
            var pos = 0;
            while ((value & 1) == 0) {
                pos++;
                value >>= 1;
            }
            return pos;
        }

        private static int CalcReverse(ulong value)
        {
            var pos = 0;
            while ((value >>= 1) != 0) {
                pos++;
            }
            return pos;
        }

        [TestMethod]
        public void BitScanner_Mask()
        {
            Assert.AreEqual(0U, BitScanner.Mask(0));
            Assert.AreEqual(1U, BitScanner.Mask(1));
            Assert.AreEqual(3U, BitScanner.Mask(2));
            Assert.AreEqual(3U, BitScanner.Mask(3));
            Assert.AreEqual(7U, BitScanner.Mask(4));
            Assert.AreEqual(7U, BitScanner.Mask(5));
            Assert.AreEqual(7U, BitScanner.Mask(6));
            Assert.AreEqual(7U, BitScanner.Mask(7));
            Assert.AreEqual(0x7FFFFFFFU, BitScanner.Mask(0x7FFFFFFF));
            Assert.AreEqual(0xFFFFFFFFU, BitScanner.Mask(0x80000000));
            Assert.AreEqual(0xFFFFFFFFU, BitScanner.Mask(0x80000001));
            Assert.AreEqual(0xFFFFFFFFU, BitScanner.Mask(0xFFFFFFFF));
        }

        [TestMethod]
        public void BitScanner_Scan()
        {
            Test(1);
            Test(3);
            Test(4);
            Test(7);
            Test(int.MaxValue);
            Test(uint.MaxValue);
            for (var trial = 0; trial < 1000000; ++trial) {
                Test((uint)rng.Next());
            }
        }
    }
}
