using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MersenneTwister.Tests
{
    [TestClass]
    public class SequenceTest
    {
        private void Test(Random expected, Random actual)
        {
            for (var i = 0; i < 1000000; ++i) {
                Assert.AreEqual(expected.Next(), actual.Next());
            }
            for (var i = 0; i < 1000000; ++i) {
                Assert.AreEqual(expected.NextDouble(), actual.NextDouble());
            }
            var bufe = new byte[727];
            var bufa = new byte[bufe.Length];
            for (var i = 0; i < 10000; ++i) {
                expected.NextBytes(bufe);
                actual.NextBytes(bufa);
                CollectionAssert.AreEqual(bufe, bufa);
            }
        }

        [TestMethod]
        public void Sequence_MT()
        {
            var seed = Environment.TickCount;
            Test(MTRandom.Create(seed, MTEdition.Original_19937), MTRandom.Create(seed, MTEdition.Cok_19937));
            Test(MTRandom.Create(seed, MTEdition.Original_19937), MTRandom.Create(seed, MTEdition.CokOpt_19937));
        }

        [TestMethod]
        public void Sequence_SFMT()
        {
            var seed = Environment.TickCount;
            Test(SfmtRandom.Create(seed, SfmtEdition.Original_19937), SfmtRandom.Create(seed, SfmtEdition.Opt_19937));
        }

        [TestMethod]
        public void Sequence_dSFMT()
        {
            var seed = Environment.TickCount;
            Test(DsfmtRandom.Create(seed, DsfmtEdition.Original_19937), DsfmtRandom.Create(seed, DsfmtEdition.Opt_19937));
            Test(DsfmtRandom.Create(seed, DsfmtEdition.Original_19937), DsfmtRandom.Create(seed, DsfmtEdition.OptGen_19937));
        }
    }
}
