//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.AspNetCore.Mvc;
//using BaseTemplate.Shared.Dtos.SystemDtos;
//using BaseTemplate.Shared.Exceptions;

//namespace BaseTemplate.Presentation.Filters
//{
//    public class ValidationFilter : IActionFilter
//    {
//        public void OnActionExecuting(ActionExecutingContext context)
//        {
//            if (!context.ModelState.IsValid)
//            {
//                var errors = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
//                context.Result = new BadRequestObjectResult(ServiceResult<NoContentResult>.Fail(400, errors));
//            }
//        }

//        public void OnActionExecuted(ActionExecutedContext context) { }
//    }
//}

using BaseTemplate.Shared.Dtos.SystemDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BaseTemplate.Presentation.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        private readonly ILogger<ValidationFilter> _logger;

        public ValidationFilter(ILogger<ValidationFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();

                _logger.LogWarning("Request Başarısız Validasyon Hatası  {RequestPath}. Errors: {Errors}",
                    context.HttpContext.Request.Path, string.Join(", ", errors));

                // Return the validation error response
                context.Result = new BadRequestObjectResult(ServiceResult<NoContentResult>.Fail(400, errors));
                return;
            }

            await next();
        }
    }
}
