using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MersenneTwister.Tests
{
    [TestClass]
    public class RandomTest
    {
        private static void Repeat(Action action)
        {
            for (var trial = 0; trial < 100000; ++trial) {
                action();
            }
        }

        private void Test(Random rng)
        {
            Repeat(delegate { Assert.AreEqual(0, rng.Next(0)); });
            Repeat(delegate { Assert.AreEqual(0, rng.Next(1)); });
            Repeat(delegate { Assert.IsTrue(rng.Next(2) < 2); });
            Repeat(delegate { Assert.AreEqual(0, rng.Next(0, 1)); });
            Repeat(delegate { Assert.AreEqual(1, rng.Next(1, 2)); });
            Repeat(delegate { Assert.IsTrue(rng.Next(0, 2) < 2); });
            Repeat(delegate { Assert.AreEqual(int.MaxValue - 1, rng.Next(int.MaxValue - 1, int.MaxValue)); });
            Repeat(delegate { Assert.IsTrue(0 <= rng.NextDouble()); });
            Repeat(delegate { Assert.IsTrue(1 > rng.NextDouble()); });
            Repeat(delegate { Assert.IsTrue(0 <= rng.Next(int.MaxValue)); });
            Repeat(delegate { Assert.AreEqual(-1, rng.Next(-1, -1)); });
            Repeat(delegate { Assert.AreEqual(-1, rng.Next(-1, -0)); });
            Repeat(delegate { Assert.AreEqual(int.MinValue, rng.Next(int.MinValue, int.MinValue + 1)); });
            var meta = Randoms.FastestInt32;
            Repeat(delegate {
                var n0 = meta.Next(2);
                var n1 = meta.Next(2);
                var n2 = meta.Next(2);
                var nd = meta.Next(2);
                Assert.IsTrue(n0 >= 0 && n0 <= 1);
                Assert.IsTrue(n1 >= 0 && n1 <= 1);
                Assert.IsTrue(n2 >= 0 && n2 <= 1);
                Assert.IsTrue(nd >= 0 && nd <= 1);
                if (n0 == 0) { rng.Next(); }
                if (n1 == 0) { rng.Next(15); }
                if (n2 == 0) { rng.Next(-19, 31); }
                if (nd == 0) { rng.NextDouble(); }
            });
            TestNextBytes(rng, 0);
            TestNextBytes(rng, 1);
            TestNextBytes(rng, 2);
            TestNextBytes(rng, 4);
            TestNextBytes(rng, 7);
            TestNextBytes(rng, 8);
            TestNextBytes(rng, 9);
            TestNextBytes(rng, 15);
            TestNextBytes(rng, 16);
            TestNextBytes(rng, 17);
            TestNextBytes(rng, 127);
            TestNextBytes(rng, 128);
            TestNextBytes(rng, 129);
            TestNextBytes(rng, 255);
            TestNextBytes(rng, 256);
            TestNextBytes(rng, 257);
        }

        private void TestNextBytes(Random rng, int byteSize)
        {
            var buf = new byte[byteSize];
            var nzf = new bool[byteSize];
            for (var trial = 0; trial < 256; ++trial) {
                rng.NextBytes(buf);
                var num = 0;
                for (var i = 0; i < byteSize; ++i) {
                    nzf[i] |= (buf[i] != 0);
                    num += (nzf[i] ? 1 : 0);
                }
                if (num == byteSize) {
                    return;
                }
            }
            Assert.Fail("suspicious uninitialized bytes are detected");
        }

        [TestMethod]
        public void Random_MT()
        {
            Test(MTRandom.Create(MTEdition.Original_19937));
            Test(MTRandom.Create(MTEdition.Cok_19937));
            Test(MTRandom.Create(MTEdition.CokOpt_19937));
        }

        [TestMethod]
        public void Random_MT64()
        {
            Test(MT64Random.Create(MT64Edition.Original_19937));
            Test(MT64Random.Create(MT64Edition.Opt_19937));
        }

        [TestMethod]
        public void Random_SFMT()
        {
            Test(SfmtRandom.Create(SfmtEdition.Original_19937));
            Test(SfmtRandom.Create(SfmtEdition.Opt_19937));
        }

        [TestMethod]
        public void Random_dSFMT()
        {
            Test(DsfmtRandom.Create(DsfmtEdition.Original_19937));
            Test(DsfmtRandom.Create(DsfmtEdition.Opt_19937));
            Test(DsfmtRandom.Create(DsfmtEdition.OptGen_521));
            Test(DsfmtRandom.Create(DsfmtEdition.OptGen_1279));
            Test(DsfmtRandom.Create(DsfmtEdition.OptGen_2203));
            Test(DsfmtRandom.Create(DsfmtEdition.OptGen_4253));
            Test(DsfmtRandom.Create(DsfmtEdition.OptGen_11213));
            Test(DsfmtRandom.Create(DsfmtEdition.OptGen_19937));
            Test(DsfmtRandom.Create(DsfmtEdition.OptGen_44497));
            Test(DsfmtRandom.Create(DsfmtEdition.OptGen_86243));
            Test(DsfmtRandom.Create(DsfmtEdition.OptGen_132049));
            Test(DsfmtRandom.Create(DsfmtEdition.OptGen_216091));
        }

        [TestMethod]
        public void Random_Accurate()
        {
            Test(new AccurateRandom(new Random()));
        }
    }
}
