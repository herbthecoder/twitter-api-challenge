using JackHenry.Services.TwitterService.Application.Queries.GetTopHashtags;
using JackHenry.Services.TwitterService.Application.Queries.GetTweetCount;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JackHenry.Services.TwitterService.Api.Controllers
{
    [Produces("application/json")]
    public class TwitterController : BaseController
    {
        /// <summary>
        /// Gets the current number ot tweets.
        /// </summary>
        /// <returns>HTTP 200 - a json string with tweet count data.</returns>
        // GET: api/v1/tweets/
        [HttpGet("/tweets/count")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetTweetCountResponseModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetTweetCountResponseModel>> GetTweetCount()
        {
            var responseModel = await Mediator.Send(new GetTweetCountRequestModel());
            return Ok(responseModel);
        }

        /// <summary>
        /// Gets the top n hashtags.
        /// </summary>
        /// <param name="count">The number of hashtags to return.</param>
        /// <returns>HTTP 200 - a json string with top hashtags data.</returns>
        // GET: api/v1/tweets/hashtags/top
        [HttpGet("/tweets/hashtags/top")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetTopHashtagsResponseModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetTopHashtagsResponseModel>> GetTopHashTags(int count)
        {
            var responseModel = await Mediator.Send(new GetTopHashtagsRequestModel() { Count = count});
            return Ok(responseModel);
        }
    }
}
