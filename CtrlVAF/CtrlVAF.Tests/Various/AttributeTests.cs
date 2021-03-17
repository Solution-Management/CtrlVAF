using CtrlVAF.BackgroundOperations;
using CtrlVAF.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Tests.Various
{
    [TestClass]
    public class AttributeTests
    {
        [DataTestMethod]
        [DataRow(0, IntervalKind.Seconds)]
        [DataRow(0.0, IntervalKind.Seconds)]
        [DataRow(1, IntervalKind.Seconds)]
        [DataRow(29.999, IntervalKind.Seconds)]
        [DataRow(0, IntervalKind.Minutes)]
        [DataRow(0.2, IntervalKind.Minutes)]
        [DataRow(0.0, IntervalKind.Hours)]
        [DataRow(0.001, IntervalKind.Hours)]
        [DataRow(-0.1, IntervalKind.Seconds)]
        [DataRow(-0.1, IntervalKind.Minutes)]
        [DataRow(-0.1, IntervalKind.Hours)]
        [DataRow(-0.1, IntervalKind.Days)]
        public void AssertThat_RecurringAttribute_InvalidInterval_ReturnsDefault(double interval, IntervalKind intervalKind)
        {
            var expected = 600;
            var attr = new RecurringAttribute(1);
            attr.debug = true;
            var actual = attr.IntervalToSeconds(interval, intervalKind);
            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow(30, IntervalKind.Seconds, 30)]
        [DataRow(1000, IntervalKind.Seconds, 1000)]
        [DataRow(9999, IntervalKind.Seconds, 9999)]
        [DataRow(0.5, IntervalKind.Minutes, 30)]
        [DataRow(1.0, IntervalKind.Minutes, 60)]
        [DataRow(1, IntervalKind.Hours, 60 * 60)]
        [DataRow(0.5, IntervalKind.Hours, 30 * 60)]
        [DataRow(1, IntervalKind.Days, 60 * 60 * 24)]
        public void AssertThat_RecurringAttribute_ValidInterval_ReturnsInterval(double interval, IntervalKind intervalKind, int expected)
        {
            var attr = new RecurringAttribute(1);
            attr.debug = true;
            var actual = attr.IntervalToSeconds(interval, intervalKind);
            Assert.AreEqual(expected, actual);
        }
    }
}
