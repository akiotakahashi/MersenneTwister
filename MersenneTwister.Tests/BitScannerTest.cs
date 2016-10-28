using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MersenneTwister.Tests
{
    [TestClass]
    public class BitScannerTest
    {
        private readonly Random rng = DsfmtRandom.Create();

        [TestMethod]
        public void BitScanner_NB1_32()
        {
            Assert.AreEqual(0, BitScanner.NumberOfBit1(0U));
            Assert.AreEqual(1, BitScanner.NumberOfBit1(1U));
            Assert.AreEqual(1, BitScanner.NumberOfBit1(0x40000000U));
            Assert.AreEqual(2, BitScanner.NumberOfBit1(0x40000001U));
            Assert.AreEqual(31, BitScanner.NumberOfBit1(0x7FFFFFFFU));
            Assert.AreEqual(31, BitScanner.NumberOfBit1(0xFFFFFFFEU));
            Assert.AreEqual(32, BitScanner.NumberOfBit1(0xFFFFFFFFU));
        }

        [TestMethod]
        public void BitScanner_NB1_64()
        {
            Assert.AreEqual(0, BitScanner.NumberOfBit1(0UL));
            Assert.AreEqual(1, BitScanner.NumberOfBit1(1UL));
            Assert.AreEqual(1, BitScanner.NumberOfBit1(0x4000000000000000UL));
            Assert.AreEqual(2, BitScanner.NumberOfBit1(0x4000000000000001UL));
            Assert.AreEqual(63, BitScanner.NumberOfBit1(0x7FFFFFFFFFFFFFFFUL));
            Assert.AreEqual(63, BitScanner.NumberOfBit1(0xFFFFFFFFFFFFFFFEUL));
            Assert.AreEqual(64, BitScanner.NumberOfBit1(0xFFFFFFFFFFFFFFFFUL));
        }

        [TestMethod]
        public void BitScanner_NTZ_32()
        {
            Assert.AreEqual(0, BitScanner.NumberOfTrailingZeros32(0x00000001U));
            Assert.AreEqual(1, BitScanner.NumberOfTrailingZeros32(0x00000002U));
            Assert.AreEqual(0, BitScanner.NumberOfTrailingZeros32(0x00000003U));
            Assert.AreEqual(0, BitScanner.NumberOfTrailingZeros32(0x80000001U));
            Assert.AreEqual(0, BitScanner.NumberOfTrailingZeros32(0x40000001U));
            Assert.AreEqual(0, BitScanner.NumberOfTrailingZeros32(0xFFFFFFF1U));
            Assert.AreEqual(0, BitScanner.NumberOfTrailingZeros32(0xFFFFFFFFU));
            Assert.AreEqual(32, BitScanner.NumberOfTrailingZeros32(0x00000000U));
            Assert.AreEqual(31, BitScanner.NumberOfTrailingZeros32(0x80000000U));
            Assert.AreEqual(30, BitScanner.NumberOfTrailingZeros32(0x40000000U));
        }

        [TestMethod]
        public void BitScanner_NTZ_64()
        {
            Assert.AreEqual(0, BitScanner.NumberOfTrailingZeros64(0x0000000000000001UL));
            Assert.AreEqual(1, BitScanner.NumberOfTrailingZeros64(0x0000000000000002UL));
            Assert.AreEqual(0, BitScanner.NumberOfTrailingZeros64(0x0000000000000003UL));
            Assert.AreEqual(0, BitScanner.NumberOfTrailingZeros64(0x8000000000000001UL));
            Assert.AreEqual(0, BitScanner.NumberOfTrailingZeros64(0x4000000000000001UL));
            Assert.AreEqual(0, BitScanner.NumberOfTrailingZeros64(0xFFFFFFFFFFFFFFF1UL));
            Assert.AreEqual(0, BitScanner.NumberOfTrailingZeros64(0xFFFFFFFFFFFFFFFFUL));
            Assert.AreEqual(64, BitScanner.NumberOfTrailingZeros64(0x0000000000000000UL));
            Assert.AreEqual(63, BitScanner.NumberOfTrailingZeros64(0x8000000000000000UL));
            Assert.AreEqual(62, BitScanner.NumberOfTrailingZeros64(0x4000000000000000UL));
        }

        [TestMethod]
        public void BitScanner_NLZ_32()
        {
            Assert.AreEqual(32, BitScanner.NumberOfLeadingZeros32(0));
            Assert.AreEqual(31, BitScanner.NumberOfLeadingZeros32(1));
            Assert.AreEqual(30, BitScanner.NumberOfLeadingZeros32(2));
            Assert.AreEqual(30, BitScanner.NumberOfLeadingZeros32(3));
            Assert.AreEqual(0, BitScanner.NumberOfLeadingZeros32(0xFFFFFFFFU));
            Assert.AreEqual(0, BitScanner.NumberOfLeadingZeros32(0x80000000U));
            Assert.AreEqual(1, BitScanner.NumberOfLeadingZeros32(0x7FFFFFFFU));
            Assert.AreEqual(1, BitScanner.NumberOfLeadingZeros32(0x40000000U));
        }

        [TestMethod]
        public void BitScanner_NLZ_64()
        {
            Assert.AreEqual(64, BitScanner.NumberOfLeadingZeros64(0));
            Assert.AreEqual(63, BitScanner.NumberOfLeadingZeros64(1));
            Assert.AreEqual(62, BitScanner.NumberOfLeadingZeros64(2));
            Assert.AreEqual(62, BitScanner.NumberOfLeadingZeros64(3));
            Assert.AreEqual(0, BitScanner.NumberOfLeadingZeros64(0xFFFFFFFFFFFFFFFFUL));
            Assert.AreEqual(0, BitScanner.NumberOfLeadingZeros64(0x8000000000000000UL));
            Assert.AreEqual(1, BitScanner.NumberOfLeadingZeros64(0x7FFFFFFFFFFFFFFFUL));
            Assert.AreEqual(1, BitScanner.NumberOfLeadingZeros64(0x4000000000000000UL));
        }

        [TestMethod]
        public void BitScanner_Mask()
        {
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
            Assert.AreEqual(0U, BitScanner.Mask(0));
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
    }
}
