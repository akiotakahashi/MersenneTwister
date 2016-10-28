using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MersenneTwister.Tests
{
    [TestClass]
    public class AccurateRandomTest
    {
        private sealed class ConstRandom : Random
        {
            private readonly int[] array;
            private int index;

            public ConstRandom(params int[] array)
            {
                this.array = array;
                this.index = array.Length - 1;
                Array.Reverse(this.array);
            }

            public override int Next()
            {
                if (this.index == 0) {
                    this.index = this.array.Length - 1;
                    return this.array[0];
                }
                return this.array[this.index--];
            }

            public override int Next(int maxValue)
            {
                return Math.Min(maxValue, this.Next());
            }

            public override int Next(int minValue, int maxValue)
            {
                return Math.Min(maxValue, Math.Max(minValue, this.Next()));
            }

            public override void NextBytes(byte[] buffer)
            {
                for (var i = 0; i < buffer.Length; ++i) {
                    buffer[i] = (byte)this.Next();
                }
            }

            public override double NextDouble()
            {
                throw new NotSupportedException();
            }

            protected override double Sample()
            {
                throw new NotSupportedException();
            }
        }

        [TestMethod]
        public void AccurateRandom_FullPrecisionDouble_c0o1()
        {
            Random bits;
            bits = new ConstRandom(new int[] { 2, 0, 0 });
            Assert.AreEqual(BitConverter.Int64BitsToDouble(0x3FE0000000000000L), AccurateRandom.FullPrecisionDouble_c0o1(bits));
            bits = new ConstRandom(new int[] { 2, 0x7FFFFFFE, 0x7FFFFFFE });
            Assert.IsTrue(1 > AccurateRandom.FullPrecisionDouble_c0o1(bits));
            Assert.AreEqual(BitConverter.Int64BitsToDouble(0x3FE0000000000000L | ((0x3FFFFFFFL << 22) ^ 0x3FFFFFFF)), AccurateRandom.FullPrecisionDouble_c0o1(bits));
            bits = new ConstRandom(Enumerable.Repeat(0, 34).Append(0x010, 0, 0).ToArray());
            Assert.AreEqual(BitConverter.Int64BitsToDouble(0x0000000000000000), AccurateRandom.FullPrecisionDouble_c0o1(bits));
            bits = new ConstRandom(Enumerable.Repeat(0, 34).Append(0x008, 0, 0).ToArray());
            Assert.AreEqual(BitConverter.Int64BitsToDouble(0x0000000000000000), AccurateRandom.FullPrecisionDouble_c0o1(bits));
            bits = new ConstRandom(Enumerable.Repeat(0, 34).Append(0x008, 0x7FFFFFFF, 0x7FFFFFFF).ToArray());
            Assert.IsTrue(0 < AccurateRandom.FullPrecisionDouble_c0o1(bits));
            Assert.AreEqual(BitConverter.Int64BitsToDouble((0x3FFFFFFFL << 22) ^ 0x3FFFFFFF), AccurateRandom.FullPrecisionDouble_c0o1(bits));
        }

        [TestMethod]
        public void AccurateRandom_Next0()
        {
            var rng0 = new AccurateRandom(new ConstRandom(0));
            Assert.AreEqual(0, rng0.Next());
            var rng1 = new AccurateRandom(new ConstRandom(int.MaxValue - 1));
            Assert.AreEqual(int.MaxValue - 1, rng1.Next());
        }

        [TestMethod]
        public void AccurateRandom_Next1()
        {
            var rng0 = new AccurateRandom(new ConstRandom(0));
            Assert.AreEqual(0, rng0.Next(1));
            Assert.AreEqual(0, rng0.Next(3));
            Assert.AreEqual(0, rng0.Next(7));
            Assert.AreEqual(0, rng0.Next(17));
            var rng1 = new AccurateRandom(new ConstRandom(int.MaxValue - 1));
            Assert.AreEqual(0, rng1.Next(1));
            Assert.AreEqual(7, rng1.Next(8));
            Assert.AreEqual(31, rng1.Next(32));
            Assert.AreEqual(0x3FFFFFFF, rng1.Next(0x40000000));
        }

        [TestMethod]
        public void AccurateRandom_Next2()
        {
            var rng0 = new AccurateRandom(new ConstRandom(0));
            Assert.AreEqual(-1, rng0.Next(-1, 1));
            Assert.AreEqual(int.MinValue, rng0.Next(int.MinValue, int.MaxValue));
            var rng1 = new AccurateRandom(new ConstRandom(int.MaxValue - 1));
            Assert.AreEqual(0, rng1.Next(-1, 1));
            Assert.AreEqual(0x3FFFFFFF + int.MinValue, rng1.Next(int.MinValue, (int)(0x40000000L + int.MinValue)));
        }

        [TestMethod]
        public void AccurateRandom_NextDouble()
        {
            var rng0 = new AccurateRandom(new ConstRandom(0));
            Assert.AreEqual(0, rng0.NextDouble());
            var rng1 = new AccurateRandom(new ConstRandom(int.MaxValue - 1));
            Assert.AreEqual(BitConverter.Int64BitsToDouble(0x3FE0000000000000L | ((0x3FFFFFFFL << 22) ^ 0x3FFFFFFF)), rng1.NextDouble());
        }
    }

    internal static class IEnumerableExtensions
    {
        public static IEnumerable<T> Append<T>(this IEnumerable<T> collection, params T[] values)
        {
            foreach (var val in collection) {
                yield return val;
            }
            foreach (var val in values) {
                yield return val;
            }
        }
    }
}
