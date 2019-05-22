using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MersenneTwister
{
    public abstract class RandomBase : Random
    {
        public sealed override int Next()
        {
            return MathUtil.Next(this.GenerateUInt32());
        }

        public sealed override int Next(int maxValue)
        {
            return MathUtil.Next(maxValue, this.GenerateUInt32());
        }

        public sealed override int Next(int minValue, int maxValue)
        {
            return MathUtil.Next(minValue, maxValue, this.GenerateUInt32());
        }

        public sealed override void NextBytes(byte[] buffer)
        {
            this.GenerateBytes(buffer);
        }

        public sealed override double NextDouble()
        {
            return this.GenerateDouble();
        }

        protected sealed override double Sample()
        {
            return this.GenerateDouble();
        }

        protected abstract uint GenerateUInt32();
        protected abstract ulong GenerateUInt64();
        protected abstract double GenerateDouble();
        protected abstract void GenerateBytes(byte[] buffer);
    }
}
