using food_heaven_backend.Security.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace food_heaven_backend.Shared.Infraestructure.Attribute;

public class CustomAuthorizeAttribute : System.Attribute, IAsyncAuthorizationFilter
{
    private readonly string[] _roles;

    public CustomAuthorizeAttribute(params string[] roles)
    {
        _roles = roles;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.Items["User"] as User; //User1 role mkt

        if (user == null || !_roles[0].Contains(user.Subscription))
        {
            context.Result = new ForbidResult();
        }
    }

}