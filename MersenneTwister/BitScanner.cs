using System;
using System.Runtime.CompilerServices;

namespace MersenneTwister
{
#if PUBLIC
    public
#else
    internal
#endif
    static class BitScanner
    {
        public static uint Mask(uint b)
        {
            if (b == 0) { return 0; }
            var bits = (ulong)BitConverter.DoubleToInt64Bits(b);
            var exp = (int)(bits >> 52) - 1022; // = -1023 + 1
            return (uint)((1UL << exp) - 1);
        }
    }
}
