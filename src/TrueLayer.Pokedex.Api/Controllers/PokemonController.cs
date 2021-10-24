using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TrueLayer.Pokedex.Api.Middleware;
using TrueLayer.Pokedex.Application.Services;
using TrueLayer.Pokedex.Domain.Models;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TrueLayer.Pokedex.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly ILogger<PokemonController> _logger;
        private readonly IPokemonService _pokemonService;

        public PokemonController(IPokemonService pokemonService, ILogger<PokemonController> logger)
        {
            _pokemonService = pokemonService;
            _logger = logger;
        }

        [HttpGet("{name}")]
        [Produces("application/vnd.uk.co.truelayer.pokemon.detail.api-v1+json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PokemonDetail))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PokemonDetail>> GetPokemon([FromRoute] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Supplied parameters are invalid. Can not process request.");
            }

            try
            {
                var result = await _pokemonService.GetPokemonDetail(name);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong. Please check logs for more detail.");
            }
        }

        [HttpGet("translated/{name}")]
        [Produces("application/vnd.uk.co.truelayer.translated.pokemon.api-v1+json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PokemonDetail))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PokemonDetail>> GetTranslatedPokemon([FromRoute] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Supplied parameters are invalid. Can not process request.");
            }

            try
            {
                var result = await _pokemonService.GetPokemonTranslatedDetail(name);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong. Please check logs for more detail.");
            }
        }
    }
}