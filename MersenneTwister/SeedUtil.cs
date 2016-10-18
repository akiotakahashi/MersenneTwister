using System;
using System.Diagnostics;
using System.Threading;

namespace MersenneTwister
{
    internal static class SeedUtil
    {
        private static readonly Stopwatch watch = Stopwatch.StartNew();

        private static int counter = 0;

        public static uint[] GenerateSeed()
        {
            var tick = (ulong)DateTime.UtcNow.Ticks;
            var time = (ulong)watch.ElapsedTicks;
            var seed = new[] {
                (uint)tick, (uint)(tick >> 32),
                (uint)time, (uint)(time >> 32),
                (uint)Environment.TickCount,
                (uint)Environment.CurrentManagedThreadId,
                (uint)Interlocked.Increment(ref counter),
            };
            return seed;
        }
    }
}
