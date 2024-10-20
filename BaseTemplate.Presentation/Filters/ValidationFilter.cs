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


﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using BaseTemplate.Shared.Dtos.SystemDtos;

namespace BaseTemplate.Presentation.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
                context.Result = new BadRequestObjectResult(ServiceResult<NoContentResult>.Fail(400, errors));
                return;
            }

            await next();
        }
    }
}