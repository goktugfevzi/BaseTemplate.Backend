using Microsoft.AspNetCore.RateLimiting;

namespace BaseTemplate.Presentation.Configurations
{
    public static class RateLimiterConfiguration
    {
        public static void AddRateLimiter(this IServiceCollection services)
        {
            services.AddRateLimiter(o =>
            {
                o.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                o.AddFixedWindowLimiter(
                    "Fixed",
                    c =>
                    {
                        c.Window = TimeSpan.FromSeconds(5);
                        c.PermitLimit = 1;
                        c.QueueLimit = 5;
                        c.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
                    });
            });
        }
    }
}
