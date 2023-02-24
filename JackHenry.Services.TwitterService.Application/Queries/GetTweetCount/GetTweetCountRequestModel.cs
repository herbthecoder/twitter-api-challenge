using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackHenry.Services.TwitterService.Application.Queries.GetTweetCount
{
    public class GetTweetCountRequestModel : IRequest<GetTweetCountResponseModel>
    {
        public int Count { get; set; }
    }
}
