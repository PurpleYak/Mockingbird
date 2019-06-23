using Microsoft.AspNetCore.Builder;

namespace Mockingbird
{
    public static class MockingbirdMiddlewareExtension
    {
        public static IApplicationBuilder UseMockingbird(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MockingbirdMiddleware>();
        }
    }
}
