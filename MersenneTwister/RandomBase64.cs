using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MersenneTwister
{
    public abstract class RandomBase64 : RandomBase
    {
        private bool sflag32;
        private uint stock32;

        protected sealed override uint GenerateUInt32()
        {
            if (this.sflag32) {
                var val = this.stock32;
                this.sflag32 = false;
                return val;
            }
            var r = this.GenerateUInt64();
            this.stock32 = (uint)(r >> 32);
            this.sflag32 = true;
            return (uint)r;
        }

        protected override double GenerateDouble()
        {
            return MathUtil.UInt64ToDouble_c0o1(this.GenerateUInt64());
        }

        protected sealed override void GenerateBytes(byte[] buffer)
        {
            if (buffer == null) { throw new ArgumentNullException(); }
            ulong val;
            var i = 7;
            for (; i < buffer.Length; i += 8) {
                val = this.GenerateUInt64();
                buffer[i - 7] = (byte)val;
                buffer[i - 6] = (byte)(val >> 8);
                buffer[i - 5] = (byte)(val >> 16);
                buffer[i - 4] = (byte)(val >> 24);
                buffer[i - 3] = (byte)(val >> 32);
                buffer[i - 2] = (byte)(val >> 40);
                buffer[i - 1] = (byte)(val >> 48);
                buffer[i - 0] = (byte)(val >> 56);
            }
            i -= 7;
            val = this.GenerateUInt64();
            switch (i) {
            case 7:
                buffer[i] = Shift(ref val);
                goto case 6;
            case 6:
                buffer[i] = Shift(ref val);
                goto case 5;
            case 5:
                buffer[i] = Shift(ref val);
                goto case 4;
            case 4:
                buffer[i] = Shift(ref val);
                goto case 3;
            case 3:
                buffer[i] = Shift(ref val);
                goto case 2;
            case 2:
                buffer[i] = Shift(ref val);
                goto case 1;
            case 1:
                buffer[i] = Shift(ref val);
                break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte Shift(ref ulong value)
        {
            var r = (byte)value;
            value >>= 8;
            return r;
        }
    }
}
