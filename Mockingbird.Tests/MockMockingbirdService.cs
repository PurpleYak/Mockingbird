using System.Net.Http;

namespace Mockingbird.Tests
{
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
}
