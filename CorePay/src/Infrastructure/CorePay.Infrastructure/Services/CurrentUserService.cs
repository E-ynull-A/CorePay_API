using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Exceptions;
using CorePay.Domain.Utilities.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CorePay.Infrastructure.Services
{
    public class CurrentUserService:ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContext;

        public CurrentUserService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public Guid GetUserId()
        {
            string? userId = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException("Current user was not found");

            if (!Guid.TryParse(userId, out Guid Id))
                throw new UnauthorizedAccessException("Invalid current user id");

            return Id;
        }

        public string GetUserEmail()
        {
            string? email = _httpContext.HttpContext
                                .User.FindFirstValue(ClaimTypes.Email.ToString());

            if(string.IsNullOrWhiteSpace(email))
                throw new UnauthorizedAccessException("Current user was not found");

            return email;
        }

        public string GetUserRole()
        {
            string? userRoles = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Role);

            if (string.IsNullOrWhiteSpace(userRoles))
                throw new UnauthorizedAccessException("Current user was not found");

            return userRoles;
        }
    }
}
