using Microsoft.AspNetCore.Http;
using Serilog.Context;
using System.Linq;
using System.Threading.Tasks;

namespace BaseTemplate.Presentation.Middlewares
{
    public class UserInfoMiddleware
    {
        private readonly RequestDelegate _next;

        public UserInfoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userName = context.User?.Identity?.IsAuthenticated == true ? context.User.Identity.Name : null;
            var userId = context.User.Claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();

            LogContext.PushProperty("UserName", userName);
            if (!string.IsNullOrEmpty(userId))
            {
                LogContext.PushProperty("UserId", userId);
            }

            await _next(context);
        }
    }

}
