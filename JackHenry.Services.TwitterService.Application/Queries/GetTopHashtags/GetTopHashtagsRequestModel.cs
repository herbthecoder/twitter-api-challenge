using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackHenry.Services.TwitterService.Application.Queries.GetTopHashtags
{
    public class GetTopHashtagsRequestModel : IRequest<GetTopHashtagsResponseModel>
    {
        public int Count { get; set; }
    }
}
