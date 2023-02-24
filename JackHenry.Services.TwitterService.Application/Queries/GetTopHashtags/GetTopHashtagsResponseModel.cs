using JackHenry.Services.TwitterService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackHenry.Services.TwitterService.Application.Queries.GetTopHashtags
{
    public class GetTopHashtagsResponseModel
    {
        public IEnumerable<HashtagModel> Hashtags { get; set; }
    }
}
