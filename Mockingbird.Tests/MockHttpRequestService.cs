using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace Mockingbird.Tests
{
    public class MockHttpRequestService
    {
        private HttpRequest _httpRequest = Substitute.For<HttpRequest>();
        public HttpRequest Build() => _httpRequest;

        public MockHttpRequestService MockMethodReturn(string method)
        {
            _httpRequest.Method.Returns(method);
            return this;
        }

        public MockHttpRequestService MockRequestPath(string path)
        {
            _httpRequest.Path = new PathString(path);
            return this;
        }

        public MockHttpRequestService MockQueryString(string queryString)
        {
            _httpRequest.QueryString = new QueryString(queryString);
            return this;
        }
    }
}
