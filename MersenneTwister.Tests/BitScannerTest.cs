using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MersenneTwister.Tests
{
    [TestClass]
    public class BitScannerTest
    {
        private readonly Random rng = DsfmtRandom.Create();

        private static void Test(uint value) {
            Assert.AreEqual(CalcForward(value), BitScanner.LSB(value));
            Assert.AreEqual(CalcReverse(value), BitScanner.MSB(value));
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
