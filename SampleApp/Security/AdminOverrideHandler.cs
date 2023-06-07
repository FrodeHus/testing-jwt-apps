using Microsoft.AspNetCore.Authorization;

namespace SampleApp.Security;

public class AdminOverrideHandler : IAuthorizationHandler
{
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        foreach (var requirement in context.PendingRequirements)
        {
            if (context.User.IsInRole("GlobalAdmin"))
            {
                context.Succeed(requirement);
            }
        }
        return Task.CompletedTask;
    }
}
