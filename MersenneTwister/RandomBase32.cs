using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MersenneTwister
{
    public abstract class RandomBase32 : RandomBase
    {
        protected sealed override ulong GenerateUInt64()
        {
            var a = this.GenerateUInt32();
            var b = this.GenerateUInt32();
            return a | ((ulong)b << 32);
        }

        protected override double GenerateDouble()
        {
            return MathUtil.UInt32ToDouble_c0o1(this.GenerateUInt32());
        }

        protected sealed override void GenerateBytes(byte[] buffer)
        {
            if (buffer == null) { throw new ArgumentNullException(); }
            uint val;
            var i = 3;
            for (; i < buffer.Length; i += 4) {
                val = this.GenerateUInt32();
                buffer[i - 3] = (byte)val;
                buffer[i - 2] = (byte)(val >> 8);
                buffer[i - 1] = (byte)(val >> 16);
                buffer[i - 0] = (byte)(val >> 24);
            }
            i -= 3;
            val = this.GenerateUInt32();
            switch (i) {
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
        private static byte Shift(ref uint value)
        {
            var r = (byte)value;
            value >>= 8;
            return r;
        }
    }
}
