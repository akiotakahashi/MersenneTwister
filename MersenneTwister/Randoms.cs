using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace MersenneTwister
{
    public static class Randoms
    {
        private static readonly ThreadLocal<Random> wellBalanced = new ThreadLocal<Random>(() => Create(RandomType.WellBalanced), false);
        private static readonly ThreadLocal<Random> fastestInt32 = new ThreadLocal<Random>(() => Create(RandomType.FastestInt32), false);
        private static readonly ThreadLocal<Random> fastestDouble = new ThreadLocal<Random>(() => Create(RandomType.FastestDouble), false);

        private static readonly Lazy<Random> shared = new Lazy<Random>(() => Create(RandomType.WellBalanced), LazyThreadSafetyMode.ExecutionAndPublication);

        public static Random WellBalanced {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return wellBalanced.Value;
            }
        }

        public static Random FastestInt32 {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return fastestInt32.Value;
            }
        }

        public static Random FastestDouble {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return fastestDouble.Value;
            }
        }

        public static Random Create(RandomType type = RandomType.WellBalanced)
        {
            switch (type) {
            case RandomType.WellBalanced:
                return DsfmtRandom.Create();
            case RandomType.FastestInt32:
                return MT64Random.Create();
            case RandomType.FastestDouble:
                return DsfmtRandom.Create();
            default:
                throw new ArgumentException();
            }
        }

        public static Random Create(int seed, RandomType type = RandomType.WellBalanced)
        {
            switch (type) {
            case RandomType.WellBalanced:
                return DsfmtRandom.Create(seed);
            case RandomType.FastestInt32:
                return MT64Random.Create(seed);
            case RandomType.FastestDouble:
                return DsfmtRandom.Create(seed);
            default:
                throw new ArgumentException();
            }
        }

        public static int Next()
        {
            var rng = shared.Value;
            lock (rng) {
                return rng.Next();
            }
        }

        public static int Next(int maxValue)
        {
            var rng = shared.Value;
            lock (rng) {
                return rng.Next(maxValue);
            }
        }

        public static int Next(int minValue, int maxValue)
        {
            var rng = shared.Value;
            lock (rng) {
                return rng.Next(minValue, maxValue);
            }
        }

        public static double NextDouble()
        {
            var rng = shared.Value;
            lock (rng) {
                return rng.NextDouble();
            }
        }

        public static void NextBytes(byte[] buffer)
        {
            var rng = shared.Value;
            lock (rng) {
                rng.NextBytes(buffer);
            }
        }
    }

    public enum RandomType
    {
        WellBalanced,
        FastestInt32,
        FastestDouble,
    }
}
