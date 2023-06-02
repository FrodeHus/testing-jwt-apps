using Microsoft.AspNetCore.Authorization;

namespace SampleApp.Security;

public class DepartmentHandler : AuthorizationHandler<DepartmentRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DepartmentRequirement requirement)
    {
        if (context.User.HasClaim(c => c.Type == "department" && c.Value == requirement.Department))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}