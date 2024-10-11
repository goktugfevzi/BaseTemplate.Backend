using BaseTemplate.Business.Abstractions;
using BaseTemplate.Presentation.Attributes;
using BaseTemplate.Presentation.Controllers.Common;
using BaseTemplate.Schema.Dtos.ExampleDtos;
using BaseTemplate.Shared.Dtos.SystemDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaseTemplate.Presentation.Controllers
{
    [ApiController]
    public class ExampleController : BaseController
    {
        private readonly IExampleService _exampleService;

        public ExampleController(IExampleService exampleService)
        {
            _exampleService = exampleService;
        }

        [HttpGet]
        [NoNeedAuthorization]
        public async Task<IActionResult> Example()
        {
            return CreateActionResult(await _exampleService.GetAllAsync<GetExampleResponse>(x=>x.IsActive));
        }
        [HttpGet("{id}")]
        [NoNeedAuthorization]
        public async Task<IActionResult> Example(Guid id)
        {
            return CreateActionResult(await _exampleService.GetByIdAsync(id.ToString()));
        }
        [HttpPost]
        public async Task<IActionResult> Example([FromBody] CreateExampleRequest model)
        {
            return CreateActionResult(await _exampleService.AddAsync<CreateExampleRequest, NoContentDto>(model, true));
        }
        [HttpPut]
        public async Task<IActionResult> Example([FromBody] UpdateExampleRequest model)
        {
            return CreateActionResult(await _exampleService.UpdateAsync<UpdateExampleRequest, NoContentDto>(model, model.Id.ToString(), true));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Example([FromRoute] string id)
        {
            return CreateActionResult(await _exampleService.SetToPassiveAsync<NoContentDto>(id));
        }
    }
}
