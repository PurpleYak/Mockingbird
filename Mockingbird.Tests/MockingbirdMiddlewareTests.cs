using System;
using System.Collections.Generic;
using System.Net.Http;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Mockingbird.Middleware;
using NSubstitute;
using Xunit;

namespace Mockingbird.Tests
{
    public class MockingbirdEndpointProcessorTests
    {
        [Fact]
        public void RequestProcessorWithDefaultEndpointShouldReturnFalse()
        {
            var request = Substitute.For<HttpRequest>();
            var defaultEndpoint = new Models.Endpoint();

            var hasMatch = new EndpointRequestProcessor(request).EndpointMatches(defaultEndpoint);
            hasMatch.Should().Be(false);
        }

        [Fact]
        public void RequestProcessorWithNullEndpointShouldThrowArgumentNullException()
        {
            var request = Substitute.For<HttpRequest>();

            Action comparison = () => new EndpointRequestProcessor(request).EndpointMatches(null);

            comparison.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void RequestProcessorWithDisabledEndpointReturnsFalse()
        {
            var request = Substitute.For<HttpRequest>();
            var endpoint = new MockMockingbirdService()
                .SetEnabledToFalse()
                .BuildEndpoint();

            var hasMatch = new EndpointRequestProcessor(request).EndpointMatches(endpoint);
            hasMatch.Should().Be(false);
        }

        [Fact]
        public void RequestProcessorWithUnmatchingRequestMethodReturnsFalse()
        {
            var request = new MockHttpRequestService()
                .MockMethodReturn("post")
                .Build();

            var endpoints = new MockMockingbirdService()
                .SetRequest(HttpMethod.Get, "")
                .BuildEndpoint();

            var hasMatch = new EndpointRequestProcessor(request).EndpointMatches(endpoints);
            hasMatch.Should().Be(false);
        }

        [Fact]
        public void RequestProcessorShouldReturnTrue()
        {
            var request = new MockHttpRequestService()
                .MockMethodReturn("get")
                .MockRequestPath("/test")
                .MockQueryString("?id=1&name=bob")
                .Build();

            var endpoint = new MockMockingbirdService()
                .SetEnabledToTrue()
                .SetRequest(HttpMethod.Get, "test")
                .BuildEndpoint();

            var hasMatch = new EndpointRequestProcessor(request).EndpointMatches(endpoint);
            hasMatch.Should().Be(true);
        }

        [Fact]
        public void RequestProcessorWhenEndpointsDisabledShouldReturnFalse()
        {
            var request = new MockHttpRequestService()
                .MockMethodReturn("get")
                .MockRequestPath("/test")
                .MockQueryString("?id=1&name=bob")
                .Build();

            var endpoint = new MockMockingbirdService()
                    .SetEnabledToFalse()
                    .SetRequest(HttpMethod.Get, "test")
                    .BuildEndpoint();

            var hasMatch = new EndpointRequestProcessor(request).EndpointMatches(endpoint);
            hasMatch.Should().Be(false);
        }

        [Theory]
        [InlineData("get", true)]
        [InlineData("GET", true)]
        [InlineData("gET", true)]
        [InlineData("post", false)]
        [InlineData("gett", false)]
        [InlineData("delete", false)]
        [InlineData("", false)]
        [InlineData("put", false)]
        public void RequestProcessorWhenMethodDifferentCaseShouldReturnCorrectly(string method1, bool expected)
        {
            var request = new MockHttpRequestService()
                .MockMethodReturn(method1)
                .MockRequestPath("/test")
                .MockQueryString("?id=1&name=bob")
                .Build();

            var endpoint = new MockMockingbirdService()
                    .SetEnabledToTrue()
                    .SetRequest(HttpMethod.Get, "test")
                    .BuildEndpoint();

            var hasMatch = new EndpointRequestProcessor(request).EndpointMatches(endpoint);
            hasMatch.Should().Be(expected);
        }

        [Theory]
        [InlineData("test", true)]
        [InlineData("test/", true)]
        [InlineData("Test/", true)]
        [InlineData("TEST/", true)]
        [InlineData("TEST/some/PATH", true)]
        [InlineData("tets", false)]
        [InlineData("TETS", false)]
        [InlineData("", true)]
        [InlineData("/?id", true)]
        [InlineData("/?ID", true)]
        [InlineData("/?Id", true)]
        [InlineData("&name=BOB", true)]
        [InlineData("&NAME=bob", true)]
        [InlineData("&NAME=BOB", true)]
        [InlineData("&NAME=ben", false)]
        [InlineData("/?id=2&name=bob", false)]
        [InlineData("/?id=2&name=bob", false)]
        [InlineData("/test/some/path", true)]
        public void RequestProcessorWhenPathDifferentCaseItShouldMatchCorrectly(string matcher, bool expected)
        {
            var request = new MockHttpRequestService()
                .MockMethodReturn("get")
                .MockRequestPath("/test/some/path")
                .MockQueryString("?id=1&name=bob")
                .Build();

            var endpoint = new MockMockingbirdService()
                    .SetEnabledToTrue()
                    .SetRequest(HttpMethod.Get, matcher)
                    .BuildEndpoint();

            var hasMatch = new EndpointRequestProcessor(request).EndpointMatches(endpoint);
            hasMatch.Should().Be(expected);
        }
    }

    public class MockMockingbirdService
    {
        private Models.Endpoint _current = new Models.Endpoint();

        public MockMockingbirdService SetEnabledToTrue()
        {
            _current.Enabled = true;
            return this;
        }

        public MockMockingbirdService SetEnabledToFalse()
        {
            _current.Enabled = false;
            return this;
        }

        public MockMockingbirdService SetRequest(HttpMethod method, string matcher)
        {
            _current.Request = new Models.EndpointRequest
            {
                Method = method,
                PathMatcherPattern = matcher
            };
            return this;
        }

        public Models.Endpoint BuildEndpoint()
        {
            return _current;
        }
    }
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
