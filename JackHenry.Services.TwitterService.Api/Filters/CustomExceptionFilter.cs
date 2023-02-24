using JackHenry.Services.TwitterService.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace JackHenry.Services.TwitterService.Api.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            // Http Status 500 - Internal server error
            // Default error
            var code = HttpStatusCode.InternalServerError;

            // Http Status 400 - Bad Request
            // Thrown when the incoming request is malformed 
            // or unexpected.
            if (context.Exception is ValidationException)
            {
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new JsonResult(
                    ((ValidationException)context.Exception).Failures);
                return;
            }
            // Http Status 404 - Not found 
            // Thrown when a search for an entity is not found.
            else if (context.Exception is NotFoundException)
            {
                code = HttpStatusCode.NotFound;
            }
            // Log any unexpected errors
            else
            {
                //Log.Error(context.Exception, "An unexpected error occurred with the request.");
            }

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)code;
            context.Result = new JsonResult(new
            {
                statusCode = (int)code,
                statusDescription = code.ToString(),
                error = new[] { context.Exception.Message }
            });
        }
    }
}
