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
    }

    public enum RandomType
    {
        WellBalanced,
        FastestInt32,
        FastestDouble,
    }
}
