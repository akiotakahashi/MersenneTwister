using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MersenneTwister.Tests
{
    [TestClass]
    public abstract class TestBase
    {
        private readonly StringBuilder stdout = new StringBuilder();

        [TestInitialize]
        [TestCleanup]
        public void TestInit()
        {
            this.stdout.Clear();
        }

        protected static string d2s(double d)
        {
            return d.ToString("G16").PadRight(17, '0').Substring(0, 17);
        }

        protected static string d2s(double d, int digit = 15)
        {
            return d.ToString("G" + digit).PadRight(17, '0').Substring(0, digit + 2);
        }

        protected void printf(string s)
        {
            stdout.Append(s);
        }

        protected void printf(string format, params object[] args)
        {
            this.stdout.AppendFormat(format, args);
        }

        protected string GetOutput()
        {
            return this.stdout.ToString();
        }

        protected static void AssertResults(string expected, string actual, double doubleDelta)
        {
            AssertIntegerResults(expected, actual);
            AssertDoubleResults(expected, actual, doubleDelta);
        }

        protected static void AssertIntegerResults(string expected, string actual)
        {
            var me = Regex.Matches(expected, @"\b(?<!\.)\d+\b");
            var ma = Regex.Matches(actual, @"\b(?<!\.)\d+\b");
            Assert.AreEqual(me.Count, ma.Count);
            for (var i = 0; i < me.Count; ++i) {
                var e = me[i];
                var a = ma[i];
                Assert.AreEqual(ulong.Parse(e.Value), ulong.Parse(a.Value));
            }
        }

        protected static void AssertDoubleResults(string expected, string actual, double doubleDelta)
        {
            var me = Regex.Matches(expected, @"\b\d+\.\d+\b");
            var ma = Regex.Matches(actual, @"\b\d+\.\d+\b");
            Assert.AreEqual(me.Count, ma.Count);
            for (var i = 0; i < me.Count; ++i) {
                var e = me[i];
                var a = ma[i];
                Assert.AreEqual(double.Parse(e.Value), double.Parse(a.Value), doubleDelta);
            }
        }
    }
}
