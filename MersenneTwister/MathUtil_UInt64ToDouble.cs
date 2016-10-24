using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MersenneTwister
{
    partial class MathUtil
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double UInt64ToDouble_c0o1(ulong x)
        {
#if false // WRONG CALCULUS
            return  x        * (1.0 / 18446744073709551616.0 /*(1 << 64)*/);
            return (x >> 10) * (1.0 / (1UL << 54));
            return ((uint)(x >> 32) * (1.0 / (1UL << 32))) + ((uint)x * (0.5 / (1UL << 63)));
            return ((x >> 12) * (1.0 / (1UL << 52))) + ((uint)(x & 0xFFF) * (0.5 / (1UL << 63)));
#endif
            return (x >> 11) * (1.0 / (1UL << 53));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double UInt64ToDouble_o0o1(ulong x)
        {
            return ((x >> 12) + 0.5) * (1.0 / (1UL << 52));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double UInt64ToDouble_c0c1(ulong x)
        {
            return (x >> 11) * (1.0 / ((1UL << 53) - 1));
        }
    }
}
