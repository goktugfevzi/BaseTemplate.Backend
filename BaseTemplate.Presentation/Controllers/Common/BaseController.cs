using BaseTemplate.Shared.Dtos.SystemDtos;
using Microsoft.AspNetCore.Mvc;

namespace BaseTemplate.Presentation.Controllers.Common
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private ILogger<BaseController> _logger;

        protected ILogger<BaseController> Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = HttpContext.RequestServices.GetService<ILogger<BaseController>>();
                }
                return _logger;
            }
        }

        [NonAction]
        public IActionResult CreateActionResult<T>(ServiceResult<T> response)
        {
            if (response.StatusCode >= 400)
            {
                var errorMessages = response.Errors != null && response.Errors.Any()
                    ? string.Join(", ", response.Errors)
                    : "Bilinmeyen Hata.";

                Logger.LogWarning("Request Başarısız. Status Code: {StatusCode}, Errors: {Errors}, Request Path: {RequestPath}",
                    response.StatusCode, errorMessages, HttpContext.Request.Path);
            }
            else
            {
                Logger.LogInformation("Request Başarılı. Status Code: {StatusCode}, Data: {Data}, Total Items: {TotalItemCount}, Request Path: {RequestPath}",
                    response.StatusCode, response.Data != null ? response.Data.ToString() : "Veri Yok",
                    response.TotalItemCount.HasValue ? response.TotalItemCount.Value : 0,
                    HttpContext.Request.Path);
            }

            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
