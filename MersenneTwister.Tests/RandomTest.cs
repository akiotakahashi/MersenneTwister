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
            for (var trial = 0; trial < 100; ++trial) {
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
        public void Random_Precise()
        {
            Test(new PreciseRandom(new Random()));
        }
    }
}
