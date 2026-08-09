
using CorePay.Application.Common;
using CorePay.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using System.Threading.Tasks;

namespace CorePay.API.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case ValidationException validationException:

                        var errorDict = validationException.Errors.GroupBy(e => e.PropertyName)
                                            .ToDictionary(g => g.Key, g => g.Select(g => g.ErrorMessage));

                        context.Response.StatusCode = StatusCodes.Status400BadRequest;

                        await context.Response.WriteAsJsonAsync(new
                        {
                            Title = "Invalid Validation!",
                            Messages = errorDict
                        });

                        break;

                    case IdentityException identityException:
                        var iErrors = identityException.Errors.GroupBy(e => e.Code)
                            .ToDictionary(g => g.Key, g => g.Select(g => g.Description));


                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                        await context.Response.WriteAsJsonAsync(new
                        {
                            Title = "Identity Failed",
                            Messages = iErrors
                        });
                        break;

                    default:

                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                        await context.Response.WriteAsJsonAsync(new
                        {
                            Title = "Iternal Server Error"
                        });

                        break;
                }
            }
        }
    }
}
