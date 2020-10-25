using NUnit.Framework;
using System;

namespace Domain.Tests
{
    public class ApproximationTests
    {
        [TestCase(new double[] { 7, 6 }, -10, -64)]
        [TestCase(new double[] { 7, 6 }, 0, 6)]
        [TestCase(new double[] { 7, 6 }, 10, 76)]
        [TestCase(new double[] { 3, 7, 6 }, -10, 236)]
        [TestCase(new double[] { 3, 7, 6 }, 0, 6)]
        [TestCase(new double[] { 3, 7, 6 }, 10, 376)]
        [TestCase(new double[] { 4, 3, 7, 6 }, -10, -3764)]
        [TestCase(new double[] { 4, 3, 7, 6 }, 0, 6)]
        [TestCase(new double[] { 4, 3, 7, 6 }, 10, 4376)]
        [TestCase(new double[] { 5, 4, 3, 7, 6 }, -10, 46236)]
        [TestCase(new double[] { 5, 4, 3, 7, 6 }, 0, 6)]
        [TestCase(new double[] { 5, 4, 3, 7, 6 }, 10, 54376)]
        public void ComputeResultFor_WhenAllArgumentsAreProper(double[] factors, double x, int expectedResult)
        {
            var approximation = new Approximation(factors);

            var result = approximation.ComputeResultFor(x);

            Assert.That((int)result, Is.EqualTo(expectedResult));
        }
    }
}
