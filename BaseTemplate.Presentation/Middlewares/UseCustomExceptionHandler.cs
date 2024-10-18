using BaseTemplate.Shared.Dtos.SystemDtos;
using BaseTemplate.Shared.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

namespace BaseTemplate.Presentation.Middlewares
{
    public static class UseCustomExceptionHandler
    {
        public static void UseCustomException(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(config =>
            {
                config.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var statusCode = exceptionFeature.Error switch
                    {
                        ClientSideException => 400,
                        UnauthorizedAccessException => 401,
                        System.InvalidOperationException => 495,
                        _ => 500
                    };

                    var response = ServiceResult<NoContentDto>.Fail(statusCode, exceptionFeature.Error.Message);
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));

                });
            });
        }
    }
}
