using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MersenneTwister
{
#if PUBLIC
    public
#else
    internal
#endif
    static partial class MathUtil
    {
        /// <summary>[0, int.MaxValue)</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Next(uint r)
        {
            // DO NOT USE: (int)(r >> 1)
            // because Random.Next returns exclusive int.MaxValue
            // i.e. The return value must be equal or less than 0x7FFFFFFE.
            return (int)(((ulong)int.MaxValue * r) >> 32);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Next(int maxValue, uint r)
        {
            if (maxValue < 0) { throw new ArgumentOutOfRangeException(); }
            return (int)(((ulong)maxValue * r) >> 32);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Next(int minValue, int maxValue, uint r)
        {
            if (maxValue < minValue) { throw new ArgumentOutOfRangeException(); }
            var num = (ulong)((long)maxValue - minValue);
            return (int)((num * r) >> 32) + minValue;
        }
    }
}
