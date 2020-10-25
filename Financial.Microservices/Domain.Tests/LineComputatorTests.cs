using NUnit.Framework;
using System.Collections.Generic;

namespace Domain.Tests
{
    public class LineComputatorTests
    {
        [TestCaseSource(nameof(TwoLinesTestCases))]
        public void AngleOf_WhenAllIsProper(Line first, Line second, int expectedAngle)
        {
            var lineComputator = new LineComputator();

            var angle = lineComputator.AngleOf(first, second);

            Assert.That((int)angle, Is.EqualTo(expectedAngle));
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
                    36
                };

                yield return new object[]
                {
                    // y = 7x + 6
                    new Line(7, 6),
                    // y = x + 6
                    new Line(6),
                    36
                };
            }
        }
    }
}
