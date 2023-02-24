using JackHenry.Services.TwitterService.Application.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JackHenry.Services.TwitterService.Application.Interfaces
{
    /// <summary>
    /// Defines an interface for all TwitterCacheDataService providers.
    /// </summary>
    public interface ITwitterCacheDataService
    {
        /// <summary>
        /// Gets the total number of tweets from the current stream asynchronously.
        /// </summary>
        /// <returns>The total number of tweets from the current stream.</returns>
        public Task<int?> GetTweetCountAsync();

        /// <summary>
        /// Gets the top 10 hashtags from the current stream asynchronously.
        /// </summary>
        /// <returns>
        /// A list containing the top 10 hashtags from the current stream.
        /// <seealso cref="HashtagModel"/>
        /// </returns>
        public Task<IEnumerable<HashtagModel>> GetTopHashtagsAsync();
    }
}
