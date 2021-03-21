using NSubstitute;
using RestSharp;
using System;

namespace Data.Loader.Stooq.Tests
{
    public class Prerequisites
    {
        public IRestClient RestClient { get; set; } = Substitute.For<IRestClient>();
        public Func<string, Method, IRestRequest> RequestFunc { get; set; } = Substitute.For<Func<string, Method, IRestRequest>>();
        public IRestRequest RestRequest { get; set; } = Substitute.For<IRestRequest>();
        public IRestResponse RestResponse { get; set; } = Substitute.For<IRestResponse>();

        public Prerequisites()
        {
            RequestFunc(Arg.Any<string>(), Arg.Any<Method>()).Returns(RestRequest);
            RestClient.ExecuteAsync(RestRequest).Returns(RestResponse);
        }
    }
}
