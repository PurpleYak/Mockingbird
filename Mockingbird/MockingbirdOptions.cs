namespace Mockingbird
{
    public sealed class MockingbirdOptions
    {
        public bool AddMockingbirdTestingHeaderResponse { get; set; }
        public string MockingbirdTestingHeaderResponseValue { get; set; } = "X-Mockingbird-Testing";
        public string MockingbirdUIPath { get; set; } = "/mockingbird";
    }
}
