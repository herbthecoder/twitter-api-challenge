using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JackHenry.Services.TwitterService.Application.Infrastructure
{
    public class RequestPerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
           where TRequest : IRequest<TResponse>
    {
        private readonly Stopwatch _timer;

        public RequestPerformanceBehavior()
        {
            _timer = new Stopwatch();
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var name = typeof(TRequest).Name;

            _timer.Start();
            var response = await next();
            _timer.Stop();

            Log.Information("TwitterStreamService Request: {Name} ({ElapsedMilliseconds} milliseconds)", name, _timer.ElapsedMilliseconds);

            return response;
        }
    }
}
