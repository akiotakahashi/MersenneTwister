using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MersenneTwister.Benchmark.Unsafe;

namespace MersenneTwister.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Debugger.IsAttached) {
                Console.WriteLine("***************************");
                Console.WriteLine("  Run without a debugger!");
                Console.WriteLine("***************************");
                Console.WriteLine();
            }
            //
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            Console.WriteLine("Wait for getting calm 3 seconds...");
            Thread.Sleep(3000);
            //
            var fmt = "   {0,23}: {1,6:f3} s (mean = {2:f10} * int.MaxValue)";
            Func<Random, long> proci = Next<Random>;
            var seed1 = Environment.TickCount;
            var N = 300000000;
            double mean;
            Console.WriteLine();
            Console.WriteLine("Random.Next: N = {0:#,0}", N);
            //
            var t_null = Measure(NullRandom.Defalut, N, proci, out mean);
            Console.WriteLine(fmt, "Null", t_null, mean);
            var t_rand = Measure(new Random(), N, proci, out mean);
            Console.WriteLine(fmt, "Random", t_rand, mean);
            var t_mt32_org = Measure(MTRandom.Create(seed1, MTEdition.Original_19937), N, proci, out mean);
            Console.WriteLine(fmt, "MT19937", t_mt32_org, mean);
            var t_mt32_cok = Measure(MTRandom.Create(seed1, MTEdition.Cok_19937), N, proci, out mean);
            Console.WriteLine(fmt, "MT19937-cok", t_mt32_cok, mean);
            var t_mt32_opt = Measure(MTRandom.Create(seed1, MTEdition.CokOpt_19937), N, proci, out mean);
            Console.WriteLine(fmt, "MT19937-cok-opt", t_mt32_opt, mean);
            var t_mt64_org = Measure(MT64Random.Create(seed1, MT64Edition.Original_19937), N, proci, out mean);
            Console.WriteLine(fmt, "MT64-19937", t_mt64_org, mean);
            var t_mt64_opt = Measure(MT64Random.Create(seed1, MT64Edition.Opt_19937), N, proci, out mean);
            Console.WriteLine(fmt, "MT64-19937-opt", t_mt64_opt, mean);
            var t_sfmt_ptr = Measure(new SfmtRandom<unsafe_sfmt_t>(seed1), N, proci, out mean);
            Console.WriteLine(fmt, "SFMT-19937-ptr", t_sfmt_ptr, mean);
            var t_sfmt_org = Measure(SfmtRandom.Create(seed1, SfmtEdition.Original_19937), N, proci, out mean);
            Console.WriteLine(fmt, "SFMT-19937", t_sfmt_org, mean);
            var t_sfmt_opt = Measure(SfmtRandom.Create(seed1, SfmtEdition.Opt_19937), N, proci, out mean);
            Console.WriteLine(fmt, "SFMT-opt-19937", t_sfmt_opt, mean);
            var t_dsfmt_ptr = Measure(new DsfmtRandom<unsafe_dsfmt_t>(seed1), N, proci, out mean);
            Console.WriteLine(fmt, "dSFMT-19937-ptr", t_dsfmt_ptr, mean);
            var t_dsfmt_org = Measure(DsfmtRandom.Create(seed1, DsfmtEdition.Original_19937), N, proci, out mean);
            Console.WriteLine(fmt, "dSFMT-19937", t_dsfmt_org, mean);
            var t_dsfmt_opt = Measure(DsfmtRandom.Create(seed1, DsfmtEdition.Opt_19937), N, proci, out mean);
            Console.WriteLine(fmt, "dSFMT-opt-19937", t_dsfmt_opt, mean);
            var t_dsfmt_gen = Measure(DsfmtRandom.Create(seed1, DsfmtEdition.OptGen_19937), N, proci, out mean);
            Console.WriteLine(fmt, "dSFMT-opt-gen-19937", t_dsfmt_gen, mean);
            var t_accurate = Measure(new AccurateRandom(DsfmtRandom.Create(seed1, DsfmtEdition.Opt_19937)), N, proci, out mean);
            Console.WriteLine(fmt, "accurate+dSFMT-opt-19937", t_accurate, mean);
            t_dsfmt_gen = Measure(DsfmtRandom.Create(seed1, DsfmtEdition.OptGen_521), N, proci, out mean);
            Console.WriteLine(fmt, "dSFMT-opt-gen-521", t_dsfmt_gen, mean);
            t_dsfmt_gen = Measure(DsfmtRandom.Create(seed1, DsfmtEdition.OptGen_216091), N, proci, out mean);
            Console.WriteLine(fmt, "dSFMT-opt-gen-216091", t_dsfmt_gen, mean);
            //
            var buf = new byte[sizeof(int) * 10];
            var t_cprng = Measure(new System.Security.Cryptography.RNGCryptoServiceProvider(), N, rng => {
                rng.GetBytes(buf);
                var x = 0L;
                x += ToUInt32(buf, 0) >> 1;
                x += ToUInt32(buf, 4) >> 1;
                x += ToUInt32(buf, 8) >> 1;
                x += ToUInt32(buf, 12) >> 1;
                x += ToUInt32(buf, 16) >> 1;
                x += ToUInt32(buf, 20) >> 1;
                x += ToUInt32(buf, 24) >> 1;
                x += ToUInt32(buf, 28) >> 1;
                x += ToUInt32(buf, 32) >> 1;
                x += ToUInt32(buf, 36) >> 1;
                return x;
            }, out mean);
            Console.WriteLine(fmt, "RNGCrypt", t_cprng, mean);
            //
            fmt = "   {0,23}: {1,6:f3} s (mean = {2:f10})";
            Func<Random, double> procd = NextDouble<Random>;
            var seed2 = new uint[] { (uint)Environment.TickCount, (uint)Environment.WorkingSet };
            N = 200000000;
            Console.WriteLine();
            Console.WriteLine("Random.NextDouble: N = {0:#,0}", N);
            //
            t_null = Measure(NullRandom.Defalut, N, procd, out mean);
            Console.WriteLine(fmt, "Null", t_null, mean);
            t_rand = Measure(new Random(), N, procd, out mean);
            Console.WriteLine(fmt, "Random", t_rand, mean);
            t_mt32_org = Measure(MTRandom.Create(seed2, MTEdition.Original_19937), N, procd, out mean);
            Console.WriteLine(fmt, "MT19937", t_mt32_org, mean);
            t_mt32_cok = Measure(MTRandom.Create(seed2, MTEdition.Cok_19937), N, procd, out mean);
            Console.WriteLine(fmt, "MT19937-cok", t_mt32_cok, mean);
            t_mt32_opt = Measure(MTRandom.Create(seed2, MTEdition.CokOpt_19937), N, procd, out mean);
            Console.WriteLine(fmt, "MT19937-cok-opt", t_mt32_opt, mean);
            t_mt64_org = Measure(MT64Random.Create(seed1, MT64Edition.Original_19937), N, procd, out mean);
            Console.WriteLine(fmt, "MT64-19937", t_mt64_org, mean);
            t_mt64_opt = Measure(MT64Random.Create(seed1, MT64Edition.Opt_19937), N, procd, out mean);
            Console.WriteLine(fmt, "MT64-19937-opt", t_mt64_opt, mean);
            t_sfmt_ptr = Measure(new SfmtRandom<unsafe_sfmt_t>(seed2), N, procd, out mean);
            Console.WriteLine(fmt, "SFMT-19937-ptr", t_sfmt_org, mean);
            t_sfmt_org = Measure(SfmtRandom.Create(seed2, SfmtEdition.Original_19937), N, procd, out mean);
            Console.WriteLine(fmt, "SFMT-19937", t_sfmt_org, mean);
            t_sfmt_opt = Measure(SfmtRandom.Create(seed2, SfmtEdition.Opt_19937), N, procd, out mean);
            Console.WriteLine(fmt, "SFMT-opt-19937", t_sfmt_opt, mean);
            t_dsfmt_ptr = Measure(new DsfmtRandom<unsafe_dsfmt_t>(seed2), N, procd, out mean);
            Console.WriteLine(fmt, "dSFMT-19937-ptr", t_dsfmt_ptr, mean);
            t_dsfmt_org = Measure(DsfmtRandom.Create(seed2, DsfmtEdition.Original_19937), N, procd, out mean);
            Console.WriteLine(fmt, "dSFMT-19937", t_dsfmt_org, mean);
            t_dsfmt_opt = Measure(DsfmtRandom.Create(seed2, DsfmtEdition.Opt_19937), N, procd, out mean);
            Console.WriteLine(fmt, "dSFMT-opt-19937", t_dsfmt_opt, mean);
            t_dsfmt_gen = Measure(DsfmtRandom.Create(seed2, DsfmtEdition.OptGen_19937), N, procd, out mean);
            Console.WriteLine(fmt, "dSFMT-opt-gen-19937", t_dsfmt_gen, mean);
            t_accurate = Measure(new AccurateRandom(DsfmtRandom.Create(seed2, DsfmtEdition.Opt_19937)), N, procd, out mean);
            Console.WriteLine(fmt, "accurate+dSFMT-opt-19937", t_accurate, mean);
            t_dsfmt_gen = Measure(DsfmtRandom.Create(seed2, DsfmtEdition.OptGen_521), N, procd, out mean);
            Console.WriteLine(fmt, "dSFMT-opt-gen-521", t_dsfmt_gen, mean);
            t_dsfmt_gen = Measure(DsfmtRandom.Create(seed2, DsfmtEdition.OptGen_216091), N, procd, out mean);
            Console.WriteLine(fmt, "dSFMT-opt-gen-216091", t_dsfmt_gen, mean);
            //
            buf = new byte[sizeof(ulong) * 10];
            t_cprng = Measure(new System.Security.Cryptography.RNGCryptoServiceProvider(), N, rng => {
                rng.GetBytes(buf);
                var x = 0d;
                x += ToDouble(buf, 0);
                x += ToDouble(buf, 8);
                x += ToDouble(buf, 16);
                x += ToDouble(buf, 24);
                x += ToDouble(buf, 32);
                x += ToDouble(buf, 40);
                x += ToDouble(buf, 48);
                x += ToDouble(buf, 56);
                x += ToDouble(buf, 64);
                x += ToDouble(buf, 72);
                return x;
            }, out mean);
            Console.WriteLine(fmt, "RNGCrypt", t_cprng, mean);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int ToInt32(byte[] buf, int index)
        {
            return (int)ToUInt32(buf, index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint ToUInt32(byte[] buf, int index)
        {
            var a = (uint)buf[index];
            var b = (uint)buf[index + 1];
            var c = (uint)buf[index + 2];
            var d = (uint)buf[index + 3];
            return a | (b << 8) | (c << 16) | (d << 24);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long ToInt64(byte[] buf, int index)
        {
            return (long)ToUInt64(buf, index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong ToUInt64(byte[] buf, int index)
        {
            var x0 = (uint)buf[index];
            var x1 = (uint)buf[index + 1];
            var x2 = (uint)buf[index + 2];
            var x3 = (uint)buf[index + 3];
            var x4 = (uint)buf[index + 4];
            var x5 = (uint)buf[index + 5];
            var x6 = (uint)buf[index + 6];
            var x7 = (uint)buf[index + 7];
            var a = x0 | (x1 << 8) | (x2 << 16) | (x3 << 24);
            var b = x4 | (x5 << 8) | (x6 << 16) | (x7 << 24);
            return a | ((ulong)b << 32);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double ToDouble(byte[] buf, int index)
        {
            return MathUtil.UInt64ToDouble_c0o1(ToUInt64(buf, index));
        }

        private static double Measure<T>(T rng, int N, Func<T, long> proc, out double mean)
        {
            var x = 0L;
            for (var i = 0; i < 100; ++i) {
                x += proc(rng);
            }
            x = 0;
            var n = N / 10;
            var sw = Stopwatch.StartNew();
            for (var i = 0; i < n; ++i) {
                x += proc(rng);
            }
            sw.Stop();
            mean = x / (double)N / int.MaxValue;
            return sw.ElapsedTicks / (double)Stopwatch.Frequency;
        }

        private static double Measure<T>(T rng, int N, Func<T, double> proc, out double mean)
        {
            var x = 0d;
            for (var i = 0; i < 100; ++i) {
                x += proc(rng);
            }
            x = 0;
            var n = N / 10;
            var sw = Stopwatch.StartNew();
            for (var i = 0; i < n; ++i) {
                x += proc(rng);
            }
            sw.Stop();
            mean = x / N;
            return sw.ElapsedTicks / (double)Stopwatch.Frequency;
        }

        private static long Next<T>(T rng) where T : Random
        {
            var x = 0L;
            x += rng.Next();
            x += rng.Next();
            x += rng.Next();
            x += rng.Next();
            x += rng.Next();
            x += rng.Next();
            x += rng.Next();
            x += rng.Next();
            x += rng.Next();
            x += rng.Next();
            return x;
        }

        private static long Next1<T>(T rng) where T : Random
        {
            var x = 0L;
            x += rng.Next(100);
            x += rng.Next(100);
            x += rng.Next(100);
            x += rng.Next(100);
            x += rng.Next(100);
            x += rng.Next(100);
            x += rng.Next(100);
            x += rng.Next(100);
            x += rng.Next(100);
            x += rng.Next(100);
            return x;
        }

        private static long Next2<T>(T rng) where T : Random
        {
            var x = 0L;
            x += rng.Next(100);
            x += rng.Next(100);
            x += rng.Next(100);
            x += rng.Next(100);
            x += rng.Next(100);
            x += rng.Next(100);
            x += rng.Next(100);
            x += rng.Next(100);
            x += rng.Next(100);
            x += rng.Next(100);
            return x;
        }

        private static double NextDouble<T>(T rng) where T : Random
        {
            var x = 0d;
            x += rng.NextDouble();
            x += rng.NextDouble();
            x += rng.NextDouble();
            x += rng.NextDouble();
            x += rng.NextDouble();
            x += rng.NextDouble();
            x += rng.NextDouble();
            x += rng.NextDouble();
            x += rng.NextDouble();
            x += rng.NextDouble();
            return x;
        }
    }
}
