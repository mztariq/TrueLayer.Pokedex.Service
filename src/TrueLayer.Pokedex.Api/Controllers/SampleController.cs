using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TrueLayer.Pokedex.Api.Middleware;
using TrueLayer.Pokedex.Application.Services;
using TrueLayer.Pokedex.Domain.Models;
using System;
using System.Threading.Tasks;

namespace TrueLayer.Pokedex.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/[controller]")]
    public class SampleController : ControllerBase
    {
        private readonly ILogger<SampleController> _logger;
        private readonly ISampleService _sampleService;

        public SampleController(ISampleService sampleService, ILogger<SampleController> logger)
        {
            _sampleService = sampleService;
            _logger = logger;
        }

        [HttpGet]
        [Produces("application/vnd.uk.co.truelayer.template.api-v1+json")]
        public async Task<ActionResult> Get(int? userId)
        {
            try
            {
                var result = userId <= 0
                    ? throw new BusinessException("This is test exception")
                    : userId;
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Consumes("application/vnd.uk.co.truelayer.template.api-v1+json")]
        public async Task<ActionResult> Post(SomeModelData modelRequest)
        {
            try
            {
                await ProcessRequest(modelRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, Array.Empty<object>());
                throw new ApplicationException("This message could not be processed");
            }

            return Ok("User created");
        }

        private async Task ProcessRequest(SomeModelData modelRequest)
        {
            _logger.LogInformation($"Processing request for adding new user {modelRequest.FirstName}");
            await _sampleService.Process(modelRequest);
            _logger.LogInformation($"Processing completed for adding new user. {modelRequest.FirstName}");
        }
    }
}