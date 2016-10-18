/**
 * The implementation of MersenneTwister.BitScanner class is derived from
 * Chess Programming Wiki (https://chessprogramming.wikispaces.com/)
 * 
 * Contributions to https://chessprogramming.wikispaces.com/ are licensed under a
 * Creative Commons Attribution Share-Alike 3.0 License.
 * 
 * URL    : https://chessprogramming.wikispaces.com/BitScan#DeBruijnMultiplation
 * LICENSE: https://creativecommons.org/licenses/by-sa/3.0/
 */

using System;
using System.Runtime.CompilerServices;

namespace MersenneTwister
{
    public static class BitScanner
    {
        private const ulong debruijn64 = 0x03f79d71b4cb0a89;

        private static readonly byte[] index64_fwd = {
             0,  1, 48,  2, 57, 49, 28,  3,
            61, 58, 50, 42, 38, 29, 17,  4,
            62, 55, 59, 36, 53, 51, 43, 22,
            45, 39, 33, 30, 24, 18, 12,  5,
            63, 47, 56, 27, 60, 41, 37, 16,
            54, 35, 52, 21, 44, 32, 23, 11,
            46, 26, 40, 15, 34, 20, 31, 10,
            25, 14, 19,  9, 13,  8,  7,  6,
        };

        private static readonly byte[] index64_rev = {
             0, 47,  1, 56, 48, 27,  2, 60,
            57, 49, 41, 37, 28, 16,  3, 61,
            54, 58, 35, 52, 50, 42, 21, 44,
            38, 32, 29, 23, 17, 11,  4, 62,
            46, 55, 26, 59, 40, 36, 15, 53,
            34, 51, 20, 43, 31, 22, 10, 45,
            25, 39, 14, 33, 19, 30,  9, 24,
            13, 18,  8, 12,  7,  6,  5, 63,
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LSB(ulong b)
        {
            return index64_fwd[((ulong)((long)b & -(long)b) * debruijn64) >> 58];
        }

        public static int MSB(ulong b)
        {
            b |= b >> 1;
            b |= b >> 2;
            b |= b >> 4;
            b |= b >> 8;
            b |= b >> 16;
            b |= b >> 32;
            return index64_rev[(b * debruijn64) >> 58];
        }
    }
}
