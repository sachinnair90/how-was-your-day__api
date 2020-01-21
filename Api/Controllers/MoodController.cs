using BusinessLogic.DTO;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Api
{
    [Authorize]
    [ApiController, Route("[controller]")]
    public class MoodController : ControllerBase
    {
        private readonly IMoodService moodService;

        public MoodController(IMoodService moodService)
        {
            this.moodService = moodService;
        }

        [SwaggerOperation(
            Summary = "All available moods",
            Description = "Fetch all available moods for authorized user",
            OperationId = "GetMoods",
            Tags = new[] { "Moods" }
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "All available moods", typeof(IEnumerable<Mood>))]
        [Produces(MediaTypeNames.Application.Json)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await moodService.GetAllMoodsAsync());
        }
    }
}