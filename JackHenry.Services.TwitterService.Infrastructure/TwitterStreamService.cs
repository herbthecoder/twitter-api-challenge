using JackHenry.Services.TwitterService.Application.Common.Models;
using JackHenry.Services.TwitterService.Application.Interfaces;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Timers;

namespace JackHenry.Services.TwitterService.Infrastructure
{
    public class TwitterStreamService : ITwitterStreamService
    {
        // Private members
        private readonly ILogger<TwitterStreamService> _logger;
        private readonly IDatabase _redisDb;
        private Timer twitterStreamTimer;
        private const int SYNC_INTERVAL = 30;
        private const int CACHE_TIMEOUT = 60;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TwitterStreamService(ILogger<TwitterStreamService> logger, IConnectionMultiplexer redisConnection)
        {
            _logger = logger;
            _redisDb = redisConnection.GetDatabase();
        }

        public void Run()
        {
            SetTimer();
        }

        private void SetTimer()
        {
            // Create a timer with a two second interval.
            twitterStreamTimer = new Timer(SYNC_INTERVAL * 1000);
            twitterStreamTimer.Elapsed += OnTimedEvent;
            twitterStreamTimer.AutoReset = true;
            twitterStreamTimer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            const int HASHTAG_COUNT = 1000;
            const int TOP_HASHTAG_COUNT = 10;

            // Generate tweet count and set Redis cache.
            var rnd = new Random();
            var tweetCount = rnd.Next(500, 1000);
            _redisDb.StringSet("tweets:count", tweetCount, System.TimeSpan.FromSeconds(CACHE_TIMEOUT));

            // Generate Hashtags and set Redis cache.
            var hashtags = CreateRandomHashTags(HASHTAG_COUNT);
            var topHashtags = (from q in hashtags
                       orderby q.Retweets descending
                       select q).Take(TOP_HASHTAG_COUNT);

            var topHashtagsJson = JsonSerializer.Serialize(topHashtags);
            _redisDb.StringSet("tweets:top-hashtags", topHashtagsJson, System.TimeSpan.FromSeconds(CACHE_TIMEOUT));
        }

        /// <summary>
        /// Creates the random word number combination.
        /// </summary>
        /// <returns>System.String.</returns>
        public static IEnumerable<HashtagModel>CreateRandomHashTags(int count)
        {
            Random rnd = new Random();
            var hashtags = new List<HashtagModel>();
            for (int i = 1; i <= count; i++)
            {
                string[] words = { "Music", "Think", "Friend", "Peace", "Hard", "Easy" };
                int randomNumber = rnd.Next(1000000, 2000000);
                string randomTag= $"#{words[rnd.Next(0, words.Length)]}{randomNumber}";
                hashtags.Add(new HashtagModel(Guid.NewGuid(), randomTag, Math.Abs(randomTag.GetHashCode())));
            }

            return hashtags;
        }
    }
}
