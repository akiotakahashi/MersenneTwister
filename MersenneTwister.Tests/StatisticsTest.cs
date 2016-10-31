using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MersenneTwister.Tests
{
    [TestClass]
    public class StatisticsTest
    {
        private static int Seed {
            get {
#if DEBUG
                return 2016103113;
#else
                return Environment.TickCount;
#endif
            }
        }

        private static IEnumerable<Random> EnumRandoms()
        {
            var seed = Seed;
            System.Diagnostics.Trace.WriteLine("SEED: " + seed);
            yield return new Random();
            yield return MTRandom.Create(seed, MTEdition.Original_19937);
            yield return MTRandom.Create(seed, MTEdition.Cok_19937);
            yield return MTRandom.Create(seed, MTEdition.CokOpt_19937);
            yield return MT64Random.Create(seed, MT64Edition.Original_19937);
            yield return MT64Random.Create(seed, MT64Edition.Opt_19937);
            yield return SfmtRandom.Create(seed, SfmtEdition.Original_19937);
            yield return SfmtRandom.Create(seed, SfmtEdition.Opt_19937);
            yield return DsfmtRandom.Create(seed, DsfmtEdition.Original_19937);
            yield return DsfmtRandom.Create(seed, DsfmtEdition.Opt_19937);
            yield return DsfmtRandom.Create(seed, DsfmtEdition.OptGen_521);
            yield return DsfmtRandom.Create(seed, DsfmtEdition.OptGen_1279);
            yield return DsfmtRandom.Create(seed, DsfmtEdition.OptGen_2203);
            yield return DsfmtRandom.Create(seed, DsfmtEdition.OptGen_4253);
            yield return DsfmtRandom.Create(seed, DsfmtEdition.OptGen_11213);
            yield return DsfmtRandom.Create(seed, DsfmtEdition.OptGen_19937);
            yield return DsfmtRandom.Create(seed, DsfmtEdition.OptGen_44497);
            yield return DsfmtRandom.Create(seed, DsfmtEdition.OptGen_86243);
            yield return DsfmtRandom.Create(seed, DsfmtEdition.OptGen_132049);
            yield return DsfmtRandom.Create(seed, DsfmtEdition.OptGen_216091);
        }

        private const int N = 1000000;

        private static IEnumerable<T> Generate<T>(Func<T> function, int n = N)
        {
#if DEBUG
            n /= 10;
#endif
            return Enumerable.Range(0, n).Select(i => function());
        }

        [TestMethod]
        public void Stats_MinMax()
        {
#if DEBUG
            foreach (var rng in EnumRandoms()) {
                System.Diagnostics.Trace.WriteLine(rng.GetType().FullName);
                this.TestMinMax(rng);
            }
#else
            Parallel.ForEach(EnumRandoms(), rng => {
                this.TestMinMax(rng);
            });
#endif
        }

        private void TestMinMax(Random rng)
        {
            Assert.IsTrue(Generate(() => rng.Next()).Min() >= 0x00000000);
            Assert.IsTrue(Generate(() => rng.Next()).Max() <= 0x7FFFFFFE);
            Assert.IsTrue(Generate(() => rng.Next(0)).Unique() == 0x00000000);
            Assert.IsTrue(Generate(() => rng.Next(1)).Unique() == 0x00000000);
            Assert.IsTrue(Generate(() => rng.Next(int.MaxValue)).Max() >= 0x00000000);
            //
            Assert.IsTrue(Generate(() => rng.Next(0, 0)).Unique() == 0);
            Assert.IsTrue(Generate(() => rng.Next(int.MinValue, int.MinValue)).Unique() == int.MinValue);
            Assert.IsTrue(Generate(() => rng.Next(int.MaxValue, int.MaxValue)).Unique() == int.MaxValue);
            //
            Assert.IsTrue(Generate(() => rng.Next(0, int.MaxValue)).Min() >= 0);
            Assert.IsTrue(Generate(() => rng.Next(0, int.MaxValue)).Max() <= int.MaxValue - 1);
            Assert.IsTrue(Generate(() => rng.Next(int.MinValue, 0)).Min() >= int.MinValue);
            Assert.IsTrue(Generate(() => rng.Next(int.MinValue, 0)).Max() <= -1);
            Assert.IsTrue(Generate(() => rng.Next(int.MinValue, int.MaxValue)).Min() >= int.MinValue);
            Assert.IsTrue(Generate(() => rng.Next(int.MinValue, int.MaxValue)).Max() <= int.MaxValue - 1);
            //
            Assert.IsTrue(Generate(() => rng.NextDouble()).Min() >= 0);
            Assert.IsTrue(Generate(() => rng.NextDouble()).Max() < 1);
        }

        [TestMethod]
        public void Stats_Mean()
        {
            foreach (var rng in EnumRandoms()) {
                AssertMean(int.MaxValue / 2.0, Generate(() => rng.Next()));
                AssertMean(int.MaxValue / 2.0, Generate(() => rng.Next(int.MaxValue)));
                AssertMean(int.MaxValue / 2.0 / 7, Generate(() => rng.Next(int.MaxValue / 7)));
                AssertMean(int.MaxValue / 2.0 / 10181, Generate(() => rng.Next(int.MaxValue / 10181)));
                AssertMean(0, Generate(() => rng.Next(int.MinValue, int.MaxValue)));
                AssertMean(0.5, Generate(() => rng.NextDouble()));
            }
        }

        private static void AssertMean(double mu, IEnumerable<int> rng)
        {
            AssertMean(mu, rng, n => n);
        }

        private static void AssertMean(double mu, IEnumerable<double> rng)
        {
            AssertMean(mu, rng, n => n);
        }

        private static void AssertMean<T>(double mu, IEnumerable<T> rng, Func<T, double> converter)
        {
            var lv1_num = 0;
            var lv1_sum = 0d;
            var lv2_num = 0;
            var lv2_sum = 0d;
            var lv2_std = 0d;
            foreach (var val in rng) {
                lv1_num += 1;
                lv1_sum += converter(val);
                if (lv1_num >= 64) {
                    var dx = (lv1_sum / lv1_num) - mu;
                    lv2_num += 1;
                    lv2_sum += dx;
                    lv2_std += dx * dx;
                    lv1_sum = 0;
                    lv1_num = 0;
                }
            }
            var mean = lv2_sum / lv2_num + mu;
            var stdev = Math.Sqrt(lv2_std);
            var sigma = (mean - mu) / stdev;
            Assert.IsTrue(sigma <= 1);
        }

        private static readonly double SQRT_2_PI = Math.Sqrt(2 * Math.PI);

        private static double normpdf(double x, double mu, double sigma)
        {
            var alpha = (x - mu) / sigma;
            return Math.Exp(-(alpha * alpha) * 0.5) / (sigma * SQRT_2_PI);
        }
    }

    public static class IEnumerableUnique
    {
        public static T Unique<T>(this IEnumerable<T> collection)
        {
            var it = collection.GetEnumerator();
            if (!it.MoveNext()) {
                throw new ArgumentException();
            }
            var val = it.Current;
            while (it.MoveNext()) {
                if (!val.Equals(it.Current)) {
                    throw new ArgumentException("Multiple Values Found");
                }
            }
            return val;
        }
    }
}
