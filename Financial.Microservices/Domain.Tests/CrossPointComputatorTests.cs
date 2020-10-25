using NUnit.Framework;
using System.Collections.Generic;

namespace Domain.Tests
{
    public class CrossPointComputatorTests
    {
        [TestCaseSource(nameof(TwoLinesTestCases))]
        public void TwoLinesCrossPoint_WhenAllIsProper(Line firstLine, Line secondLine, Point expectedPoint)
        {
            var crossPointComputation = new CrossPointComputator();

            var crossPoint = crossPointComputation.TwoLinesCrossPoint(firstLine, secondLine);

            Assert.That(crossPoint, Is.Not.Null);
            Assert.That((int)crossPoint.X, Is.EqualTo((int)expectedPoint.X));
            Assert.That((int)crossPoint.Y, Is.EqualTo((int)expectedPoint.Y));
        }

        public static IEnumerable<object> TwoLinesTestCases
        {
            get
            {
                yield return new object[]
                {
                    // y = x + 6
                    new Line(6),
                    // y = 7x + 6
                    new Line(7, 6),
                    new Point(0, 6)
                };

                yield return new object[]
                {
                    // y = 7x + 6
                    new Line(7, 6),
                    // y = x + 6
                    new Line(6),
                    new Point(0, 6)
                };
            }
        }
    }
}
