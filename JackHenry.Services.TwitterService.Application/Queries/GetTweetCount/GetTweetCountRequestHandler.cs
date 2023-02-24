using JackHenry.Services.TwitterService.Application.Exceptions;
using JackHenry.Services.TwitterService.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace JackHenry.Services.TwitterService.Application.Queries.GetTweetCount
{
    /// <summary>
    /// Defines a MediatR request handler that handles GetTweetCount requests.
    /// </summary>
    public class GetTweetCountRequestHandler : IRequestHandler<GetTweetCountRequestModel, GetTweetCountResponseModel>
    {
        // Private members
        private readonly ILogger<GetTweetCountRequestHandler> _logger;
        private readonly ITwitterCacheDataService _twitterDataService;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="twitterDataService">The <see cref="ITwitterCacheDataService"/> service provider.</param>
        public GetTweetCountRequestHandler(ILogger<GetTweetCountRequestHandler> logger, ITwitterCacheDataService twitterDataService)
        {
            _logger = logger;
            _twitterDataService = twitterDataService;
        }

        /// <summary>
        /// Handles GetTweetCount requests.
        /// </summary>
        /// <param name="request">The <see cref="GetTweetCountRequestModel"/> request model.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A <see cref="GetTweetCountResponseModel"/> response model.</returns>
        public async Task<GetTweetCountResponseModel> Handle(GetTweetCountRequestModel request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"[{nameof(GetTweetCountRequestHandler)}-{nameof(Handle)}] Handling get tweet count request.");
            int? tweetCount = await _twitterDataService.GetTweetCountAsync();
            
            if (tweetCount == null)
                throw new NotFoundException("Tweet count not available at the moment.  This could be due to the twitter data cache is not yet synchronized. Please retry your request.");

            _logger.LogInformation($"[{nameof(GetTweetCountRequestHandler)}-{nameof(Handle)}] Tweet Count: {tweetCount}.");

            return new GetTweetCountResponseModel() { Count = (int)tweetCount };
        }
    }
}
