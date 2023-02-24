using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackHenry.Services.TwitterService.Application.Queries.GetTopHashtags
{
    /// <summary>
    /// Validates the <see cref="GetTopHashtagsRequestModel"/> request.
    /// </summary>
    public class GetTopHashtagsRequestValidator : AbstractValidator<GetTopHashtagsRequestModel>
    {
        public GetTopHashtagsRequestValidator()
        {
            RuleFor(q => q.Count)
                .GreaterThan(0)
                .LessThanOrEqualTo(10)
                .WithMessage("Count must be greater than 0 and less than 10");
        }
    }
}
