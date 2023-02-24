using JackHenry.Services.TwitterService.Application.Common.Models;
using JackHenry.Services.TwitterService.Application.Interfaces;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace JackHenry.Services.TwitterService.Infrastructure
{
    /// <summary>
    /// Redis cache provider implementation of the ITwitterCacheDataService interface.
    /// </summary>
    /// <remarks>
    //  To improve performance, we are getting our data from cache only.
    //  The assumption is that there is another process
    //  that gets the twitter stream data and set the cache values and the cache expiration
    //  is less than the frequency of the synchronization.
    /// </remarks>
    public class TwitterCacheDataService : ITwitterCacheDataService
    {
        // Private members
        private readonly ILogger<TwitterCacheDataService> _logger;
        private readonly IDatabase _redisDb;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TwitterCacheDataService(ILogger<TwitterCacheDataService> logger, IConnectionMultiplexer redisConnection)
        {
            _logger = logger;
            _redisDb = redisConnection.GetDatabase();
        }

        /// <inheritdoc cref="ITwitterCacheDataService.GetTweetCountAsync(int)"/>
        public async Task<int?> GetTweetCountAsync()
        {
            //  Get the tweet count from cache.
            //  If the cache is empty, this means that the stream process
            //  has yet synchronized the twitter stream data with Redis.
            var redisVal = await _redisDb.StringGetAsync("tweets:count");
            
            if (redisVal.IsNull)
                return null;
            
            return int.Parse(redisVal);
        }

        /// <inheritdoc cref="ITwitterCacheDataService.GetTopHashtagsAsync(int)"/>
        public async Task<IEnumerable<HashtagModel>> GetTopHashtagsAsync()
        {
            //  Get the tweet count from cache.
            //  If the cache is empty, this means that the stream process
            //  has yet synchronized the twitter stream data with Redis.
            var redisVal = await _redisDb.StringGetAsync("tweets:top-hashtags");
            
            if (redisVal.IsNull)
                return null;

            var topHashtags= JsonSerializer.Deserialize<IEnumerable<HashtagModel>>(redisVal);
            return topHashtags;
        }
    }
}