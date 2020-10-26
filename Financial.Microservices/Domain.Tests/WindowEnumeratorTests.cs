using NUnit.Framework;
using System.Linq;

namespace Domain.Tests
{
    public class WindowEnumeratorTests
    {
        [Test]
        public void WhenAllIsProper_CreateInstance()
        {
            var windowEnumerator = new WindowEnumerator<int>(new int[] { }, 10);

            Assert.That(windowEnumerator, Is.Not.Null);
        }

        [Test]
        public void WhenDataIsNotDetermined_ThrowException()
        {
            Assert.Throws<WindowEnumerator<int>.Exceptions.DataIsNotDeterminedException>(
                () => _ = new WindowEnumerator<int>(null, 10)
            );
        }

        [TestCase(0)]
        [TestCase(int.MinValue)]
        [TestCase(-1)]
        public void WhenWindowSizeIsLessThanOne_ThrowException(int invalidWindowSize)
        {
            Assert.Throws<WindowEnumerator<int>.Exceptions.WindowSizeMustBeGreaterThanZeroException>(
                () => _ = new WindowEnumerator<int>(new int[] { }, invalidWindowSize)
            );
        }

        [TestCase(1, 101, 10)]
        [TestCase(1, 5, 10)]
        [TestCase(-1000, 1000, 1)]
        [TestCase(-1000, 1000, 10)]
        public void Enumerable_WhenAllIsProper(int start, int stop, int windowSize)
        {
            var listOfNumbers = Enumerable.Range(start, stop);

            var windowEnumerator = new WindowEnumerator<int>(listOfNumbers, windowSize);

            var expectedStart = start;
            while (windowEnumerator.MoveNext())
            {
                var window = windowEnumerator.Current.ToArray();
                Assert.That(window.Length, Is.EqualTo(windowSize));
                CollectionAssert.AreEqual(Enumerable.Range(expectedStart++, windowSize), window);
            }
        }
    }
}
