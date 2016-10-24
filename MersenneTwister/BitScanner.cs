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
        public static int NumberOfBit1(uint x)
        {
            x = x - ((x >> 1) & 0x55555555);
            x = (x & 0x33333333) + ((x >> 2) & 0x33333333);
            x = (x + (x >> 4)) & 0x0F0F0F0F;
            x = x + (x << 8);
            x = x + (x << 16);
            return (int)(x >> 24);
        }

        public static int NumberOfBit1(ulong x)
        {
            return NumberOfBit1((uint)x) + NumberOfBit1((uint)(x >> 32));
        }

        public static int NumberOfLeadingZeros32(uint x)
        {
            x = x | (x >> 1);
            x = x | (x >> 2);
            x = x | (x >> 4);
            x = x | (x >> 8);
            x = x | (x >> 16);
            return NumberOfBit1(~x);
        }

        public static int NumberOfLeadingZeros64(ulong x)
        {
            x = x | (x >> 1);
            x = x | (x >> 2);
            x = x | (x >> 4);
            x = x | (x >> 8);
            x = x | (x >> 16);
            x = x | (x >> 32);
            return NumberOfBit1(~x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PositionOfMSB(uint b)
        {
            return 32 - NumberOfLeadingZeros32(b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PositionOfMSB(ulong b)
        {
            return 64 - NumberOfLeadingZeros64(b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Mask(uint b)
        {
            if (b == 0) { return 0; }
            return (((1U << (PositionOfMSB(b) - 1)) - 1) << 1) | 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Mask(ulong b)
        {
            if (b == 0) { return 0; }
            return (((1UL << (PositionOfMSB(b) - 1)) - 1) << 1) | 1;
        }
    }
}
