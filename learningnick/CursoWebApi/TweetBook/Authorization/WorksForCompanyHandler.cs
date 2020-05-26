using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace TweetBook.Authorization
{
    public class WorksForCompanyHandler : AuthorizationHandler<WorksForCompanyRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, WorksForCompanyRequirement requirement)
        {
            var emailAddress = context.User?.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

            if (emailAddress.EndsWith(requirement.DomainName))
            {
                context.Succeed(requirement);
            }
            else 
            {
                context.Fail();
            }
            return Task.CompletedTask;

        }
    }
}