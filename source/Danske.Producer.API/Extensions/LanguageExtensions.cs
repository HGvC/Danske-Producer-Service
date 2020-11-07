using Danske.Producer.API.Features.Common;
using Danske.Producer.Application.Common;
using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Danske.Producer.API.Extensions
{
    public static class LanguageExtensions
    {
        public static ActionResult ToActionResult<TResult>(this Either<Failure, TResult> either)
        {
            return either.Match<ActionResult>(
                Right: _ => new NoContentResult(),
                Left: failure => new BadRequestObjectResult(failure.FormatProblemDetails())
            );
        }

        private static CommandProblemDetails FormatProblemDetails(this Failure failure) => new CommandProblemDetails
        {
            Title = "One or more validation errors occurred.",
            Errors = new Dictionary<string, string[]> { { failure.Id, new[] { failure.Error.Message } } }
        };
    }
}