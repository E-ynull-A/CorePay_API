using CorePay.Application.Common;
using CorePay.Domain.Utilities.Enums;
using CorePay.Domain.Utilities.Errors.Common;
using Microsoft.AspNetCore.Mvc;

namespace CorePay.API.Extentions
{
    public static class ResultExtention
    {
        public static IActionResult ToActionResult(this Result result,
                                                    int successStatusCode)
        {
            if (!result.IsSuccess)
                return result.Error._getFailureResult();

            return successStatusCode._getSuccessResult();

        }

        private static IActionResult _getFailureResult(this Error error)
        {
            return error.Type switch
            {
                ErrorType.NotFound => new NotFoundObjectResult(error),
                ErrorType.Unauthorized => new UnauthorizedObjectResult(error),
                ErrorType.Conflict => new ConflictObjectResult(error),
                ErrorType.Forbidden => new ObjectResult(error)
                {
                    StatusCode = StatusCodes.Status403Forbidden,
                },
                ErrorType.Validation => new BadRequestObjectResult(error),
                ErrorType.Failure => new ObjectResult(error)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                },
                _ => new BadRequestObjectResult(error)

            };
        }

        private static IActionResult _getSuccessResult(this int statusCode)
        {
            return statusCode switch
            {
                StatusCodes.Status201Created => new StatusCodeResult(StatusCodes.Status201Created),
                StatusCodes.Status204NoContent => new NoContentResult(),
                _ => new StatusCodeResult(StatusCodes.Status200OK)
            };
        }
    }
}
