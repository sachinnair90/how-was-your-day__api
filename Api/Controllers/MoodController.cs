using Api.Parameters;
using AutoMapper;
using BusinessLogic.DTO;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api
{
    [Authorize]
    [ApiController, Route("[controller]")]
    public class MoodController : ControllerBase
    {
        private readonly IMoodService moodService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public MoodController(IMoodService moodService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.moodService = moodService;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }

        [SwaggerOperation(
            Summary = "All available moods",
            Description = "Fetch all available moods",
            OperationId = "GetMoods",
            Tags = new[] { "Moods" }
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "All available moods", typeof(IEnumerable<Mood>))]
        [Produces(MediaTypeNames.Application.Json)]
        [HttpGet(Name = "GetMoods")]
        public async Task<IActionResult> Get()
        {
            return Ok(await moodService.GetAllMoodsAsync());
        }

        //[SwaggerOperation(
        //    Summary = "Moods for the user",
        //    Description = "Fetch all available moods for authorized user",
        //    OperationId = "GetMoodsForUser",
        //    Tags = new[] { "Moods" }
        //)]
        //[SwaggerResponse(StatusCodes.Status200OK, "All available moods for user", typeof(IEnumerable<UserMoodDetails>))]
        //[Produces(MediaTypeNames.Application.Json)]
        //[HttpGet(Name = "GetMoodsForUser")]
        //[Route("filter")]
        //public async Task<IActionResult> GetMoodsForUser([FromQuery]FilterMoodRequestParameter parameter)
        //{
        //    var filterMood = mapper.Map<FilterMoodRequestParameter, FilterMoodParameter>(parameter);

        //    filterMood.UserId = int.Parse(httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

        //    return Ok(await moodService.GetMoodsForUser(filterMood));
        //}
    }
}