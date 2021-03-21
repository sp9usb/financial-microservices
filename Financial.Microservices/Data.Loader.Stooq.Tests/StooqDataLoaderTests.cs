using NSubstitute;
using NUnit.Framework;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace Data.Loader.Stooq.Tests
{
    [TestFixture]
    public class StooqDataLoaderTests
    {
        [Test]
        public async Task GetCurrentValueFor_WhenAllAreProper()
        {
            var prerequisites = new Prerequisites();
            prerequisites.RestResponse.StatusCode.Returns(System.Net.HttpStatusCode.OK);
            prerequisites.RestResponse.Content.Returns("MSI.US,20210319,210400,181.79,184.52,180.08,183.22,1306876");
            var loader = new StooqDataLoader(prerequisites.RestClient, prerequisites.RequestFunc);

            var result = await loader.GetCurrentValueFor("msi.us");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Symbol, Is.EqualTo("MSI.US"));
            Assert.That(result.Date, Is.EqualTo(new DateTime(2021, 03, 19, 21, 04, 00)));
            Assert.That(result.Open, Is.InRange(181.79 - 0.01, 181.79 + 0.01));
            Assert.That(result.High, Is.InRange(184.52 - 0.01, 184.52 + 0.01));
            Assert.That(result.Low, Is.InRange(180.08 - 0.01, 180.08 + 0.01));
            Assert.That(result.Close, Is.InRange(183.22 - 0.01, 183.22 + 0.01));
            Assert.That(result.Volume, Is.EqualTo(1306876));
            prerequisites.RequestFunc.Received()("/q/l", Method.GET);
            prerequisites.RestRequest.Received().AddQueryParameter("s", "msi.us");
            _ = prerequisites.RestClient.Received().ExecuteAsync(prerequisites.RestRequest);
        }

        [Test]
        public async Task GetCurrentValueFor_WhenReceivedErrorFromServer_ThrowException()
        {
            var prerequisites = new Prerequisites();
            prerequisites.RestResponse.StatusCode.Returns(System.Net.HttpStatusCode.OK);
            prerequisites.RestResponse.ErrorException.Returns(new Exception("some exception"));
            var loader = new StooqDataLoader(prerequisites.RestClient, prerequisites.RequestFunc);

            Assert.ThrowsAsync<Exception>(() => _ = loader.GetCurrentValueFor("msi.us"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public async Task GetCurrentValueFor_WhenReceivedDataIsEmpty_ThrowException(string emptyData)
        {
            var prerequisites = new Prerequisites();
            prerequisites.RestResponse.StatusCode.Returns(System.Net.HttpStatusCode.OK);
            prerequisites.RestResponse.Content.Returns(emptyData);
            var loader = new StooqDataLoader(prerequisites.RestClient, prerequisites.RequestFunc);

            Assert.ThrowsAsync<ArgumentException>(() => _ = loader.GetCurrentValueFor("msi.us"));
        }
    }
}
