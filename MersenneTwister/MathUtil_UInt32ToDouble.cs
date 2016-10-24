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
        public static double UInt32ToDouble_c0c1(uint a, uint b)
        {
            return ((a << 21) ^ b) * (1.0 / ((1UL << 53) - 1));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double UInt32ToDouble_c0o1(uint a, uint b)
        {
            return ((a << 21) ^ b) * (1.0 / (1UL << 53));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double UInt32ToDouble_o0o1(uint a, uint b)
        {
            return (((a << 20) ^ b) + 0.5) * (1.0 / (1UL << 51));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double UInt32ToDouble_c0c1(uint x)
        {
            return x * (1.0 / 4294967295.0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double UInt32ToDouble_c0o1(uint x)
        {
            return x * (1.0 / 4294967296.0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double UInt32ToDouble_o0o1(uint x)
        {
            return (x + 0.5) * (1.0 / 4294967296.0);
        }
    }
}
