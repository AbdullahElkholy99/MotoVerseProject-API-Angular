using Microsoft.AspNetCore.Mvc.Filters;
using MotoVerse.Core.Features.CurrentUserFeature.Queries.Models;

namespace MotoVerse.Core.Filters;

public class AuthFilter : IAsyncActionFilter
{
    private readonly IMediator _mediator;
    private readonly UserManager<User> _userManager;
    public AuthFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.HttpContext.User.Identity.IsAuthenticated == true)
        {
            var roles = await _mediator.Send(new GetCurrentUserRolesQuery());
            if (roles.Data.All(x => x != "User"))
            {
                context.Result = new ObjectResult("Forbidden")
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
            else
            {
                await next();
            }

        }
    }
}

