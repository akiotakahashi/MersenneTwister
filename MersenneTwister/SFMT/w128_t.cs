using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using uint32_t = System.UInt32;
using uint64_t = System.UInt64;

namespace MersenneTwister.SFMT
{
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct w128_t
    {
        [FieldOffset(0)]
        public uint32_t u32_0;
        [FieldOffset(4)]
        public uint32_t u32_1;
        [FieldOffset(8)]
        public uint32_t u32_2;
        [FieldOffset(12)]
        public uint32_t u32_3;

        [FieldOffset(0)]
        public uint64_t u64_0;
        [FieldOffset(8)]
        public uint64_t u64_1;

        [FieldOffset(0)]
        public double d0;
        [FieldOffset(8)]
        public double d1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint32_t u32(uint index)
        {
            switch (index) {
            case 0:
                return u32_0;
            case 1:
                return u32_1;
            case 2:
                return u32_2;
            case 3:
                return u32_3;
            default:
                throw new ArgumentOutOfRangeException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void u32(uint index, uint32_t value)
        {
            switch (index) {
            case 0:
                u32_0 = value;
                return;
            case 1:
                u32_1 = value;
                return;
            case 2:
                u32_2 = value;
                return;
            case 3:
                u32_3 = value;
                return;
            default:
                throw new ArgumentOutOfRangeException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void add32(uint index, uint32_t value)
        {
            switch (index) {
            case 0:
                u32_0 += value;
                return;
            case 1:
                u32_1 += value;
                return;
            case 2:
                u32_2 += value;
                return;
            case 3:
                u32_3 += value;
                return;
            default:
                throw new ArgumentOutOfRangeException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void xor32(uint index, uint32_t value)
        {
            switch (index) {
            case 0:
                u32_0 ^= value;
                return;
            case 1:
                u32_1 ^= value;
                return;
            case 2:
                u32_2 ^= value;
                return;
            case 3:
                u32_3 ^= value;
                return;
            default:
                throw new ArgumentOutOfRangeException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint64_t u64(uint index)
        {
            switch (index) {
            case 0:
                return u64_0;
            case 1:
                return u64_1;
            default:
                throw new ArgumentOutOfRangeException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void u64(uint index, uint64_t value)
        {
            switch (index) {
            case 0:
                u64_0 = value;
                return;
            case 1:
                u64_1 = value;
                return;
            default:
                throw new ArgumentOutOfRangeException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void add64(uint index, uint64_t value)
        {
            switch (index) {
            case 0:
                u64_0 += value;
                return;
            case 1:
                u64_1 += value;
                return;
            default:
                throw new ArgumentOutOfRangeException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void xor64(uint index, uint64_t value)
        {
            switch (index) {
            case 0:
                u64_0 ^= value;
                return;
            case 1:
                u64_1 ^= value;
                return;
            default:
                throw new ArgumentOutOfRangeException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double d(uint index)
        {
            switch (index) {
            case 0:
                return d0;
            case 1:
                return d1;
            default:
                throw new ArgumentOutOfRangeException();
            }
        }
    }
}
