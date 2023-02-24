using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackHenry.Services.TwitterService.Application.Common.Models
{
    /// <summary>
    /// Represents a hasthtag model
    /// </summary>
    public class HashtagModel
    {
        /// <summary>
        /// Constructor used to initialize model.
        /// </summary>
        /// <param name="id">The unique hashtag id.</param>
        /// <param name="tag">The hashtag string.</param>
        public HashtagModel(Guid id, string tag, int retweets)
        {
            Id = id;
            Tag = tag;
            DateCreated = DateTime.Now;
            Retweets = retweets;
        }

        /// <summary>
        /// Gets the unique hashtag id.
        /// </summary>
        public Guid Id { get; }
        
        /// <summary>
        /// Get the hashtag string.
        /// </summary>
        public string Tag { get; }
        
        /// <summary>
        /// Gets the hashtag creation date.
        /// </summary>
        public DateTime DateCreated { get;  }

        /// <summary>
        /// Gets the # of hashtag retweets.
        /// </summary>
        public int Retweets { get; }
    }
}
