using JackHenry.Services.TwitterService.Application.Exceptions;
using JackHenry.Services.TwitterService.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace JackHenry.Services.TwitterService.Application.Queries.GetTopHashtags
{
    /// <summary>
    /// Defines a MediatR request handler that handles GetTopHashtags requests.
    /// </summary>
    public class GetTopHashtagsRequestHandler : IRequestHandler<GetTopHashtagsRequestModel, GetTopHashtagsResponseModel>
    {
        private readonly ILogger<GetTopHashtagsRequestHandler> _logger;
        private readonly ITwitterCacheDataService _twitterDataService;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="twitterDataService">The <see cref="ITwitterCacheDataService"/> service provider.</param>
        public GetTopHashtagsRequestHandler(ILogger<GetTopHashtagsRequestHandler> logger, ITwitterCacheDataService twitterDataService)
        {
            _logger = logger;
            _twitterDataService = twitterDataService;
        }

        /// <summary>
        /// Handles the GetTopHashtages request.
        /// </summary>
        /// <param name="request">The <see cref="GetTopHashtagsRequestModel"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<GetTopHashtagsResponseModel> Handle(GetTopHashtagsRequestModel request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Handling GetTopHashtags request...");

            var hashtags = await _twitterDataService.GetTopHashtagsAsync();
            
            if (hashtags == null)
                throw new NotFoundException("Tweet top hashtags not available at the moment. " +
                    "This could be due to the twitter data cache is not yet synchronized. Please retry your request.");

            return new GetTopHashtagsResponseModel() { Hashtags = hashtags };
        }
    }
}
