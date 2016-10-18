using System;
using System.Runtime.CompilerServices;

using uint32_t = System.UInt32;
using uint64_t = System.UInt64;

namespace MersenneTwister.SFMT
{
    public abstract class sfmt_base_t
    {
        protected sfmt_base_t()
        {
            if (!BitConverter.IsLittleEndian) {
                throw new PlatformNotSupportedException("MersenneTwister does not support Big Endian platforms");
            }
        }

        [System.Diagnostics.Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void assert(bool condition)
        {
            System.Diagnostics.Debug.Assert(condition);
        }

        protected static void copy32(w128_t[] src, uint32_t[] dst)
        {
            for (var i = 0; i < src.Length; ++i) {
                var x = src[i];
                var j = i * 4;
                dst[j + 0] = x.u32_0;
                dst[j + 1] = x.u32_1;
                dst[j + 2] = x.u32_2;
                dst[j + 3] = x.u32_3;
            }
        }

        protected static void copy64(w128_t[] src, uint64_t[] dst)
        {
            for (var i = 0; i < src.Length; ++i) {
                var x = src[i];
                var j = i * 2;
                dst[j + 0] = x.u64_0;
                dst[j + 1] = x.u64_1;
            }
        }

        protected static void copy64(w128_t[] src, double[] dst)
        {
            for (var i = 0; i < src.Length; ++i) {
                var x = src[i];
                var j = i * 2;
                dst[j + 0] = x.d0;
                dst[j + 1] = x.d1;
            }
        }

        protected static void copy64(uint64_t[] src, double[] dst)
        {
            for (var i = 0; i < src.Length; ++i) {
                dst[i] = BitConverter.Int64BitsToDouble((long)src[i]);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static uint32_t get32(w128_t[] state, uint index)
        {
            return state[index >> 2].u32(index & 3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void set32(w128_t[] state, uint index, uint32_t value)
        {
            state[index >> 2].u32(index & 3, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void add32(w128_t[] state, uint index, uint32_t value)
        {
            state[index >> 2].add32(index & 3, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void xor32(w128_t[] state, uint index, uint32_t value)
        {
            state[index >> 2].xor32(index & 3, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static uint64_t get64(w128_t[] state, uint index)
        {
            return state[index >> 1].u64(index & 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void set64(w128_t[] state, uint index, uint64_t value)
        {
            state[index >> 1].u64(index & 1, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void add64(w128_t[] state, uint index, uint64_t value)
        {
            state[index >> 1].add64(index & 1, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void xor64(w128_t[] state, uint index, uint64_t value)
        {
            state[index >> 1].xor64(index & 1, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static double getd(w128_t[] state, uint index)
        {
            return state[index >> 1].d(index & 1);
        }

        private static readonly int[] shift = new int[] { 0, 32 };
        private static readonly uint64_t[] mask = new uint64_t[] { ~0xFFFFFFFFUL, ~0xFFFFFFFF00000000UL };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static uint32_t get32(uint64_t[] state, uint index)
        {
            return (uint32_t)(state[index >> 1] >> shift[index & 1]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void set32(uint64_t[] state, uint index, uint32_t value)
        {
            var flg = index & 1;
            index >>= 1;
            var m = mask[flg];
            var s = shift[flg];
            state[index] = (state[index] & m) | ((uint64_t)value << s);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void add32(uint64_t[] state, uint index, uint32_t value)
        {
            var flg = index & 1;
            index >>= 1;
            var m = mask[flg];
            var s = shift[flg];
            state[index] = (state[index] & m) | ((uint64_t)((uint32_t)(state[index] >> s) + value) << s);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void xor32(uint64_t[] state, uint index, uint32_t value)
        {
            var flg = index & 1;
            index >>= 1;
            var m = mask[flg];
            var s = shift[flg];
            state[index] = (state[index] & m) | ((uint64_t)((uint32_t)(state[index] >> s) ^ value) << s);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static uint64_t get64(uint64_t[] state, uint index)
        {
            return state[index];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void set64(uint64_t[] state, uint index, uint64_t value)
        {
            state[index] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static double getd(uint64_t[] state, uint index)
        {
            return BitConverter.Int64BitsToDouble((long)state[index]);
        }
    }
}
