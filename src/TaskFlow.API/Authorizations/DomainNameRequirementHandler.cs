using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TaskFlow.API.Authorizations
{

    public class DomainNameRequirementHandler : AuthorizationHandler<DomainNameRequirement>
    {
        private readonly ILogger<DomainNameRequirementHandler> _logger;

        public DomainNameRequirementHandler(ILogger<DomainNameRequirementHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, DomainNameRequirement requirement)
        {

            if(context.User.Identity.IsAuthenticated)
            {
                String authenticatedUserEmail = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                if (String.IsNullOrEmpty(authenticatedUserEmail))
                {
                    _logger.LogError("Authorization failed: No email claim found for the authenticated user.");
                    context.Fail();
                    return;
                }


                if (authenticatedUserEmail.Contains(requirement.RequiredDomain))
                {
                    _logger.LogInformation("Authorization succeeded: Required domain matches the expected domain.");
                    context.Succeed(requirement);
                }
                else
                {
                    _logger.LogError("Authorization failed: Required domain does not match the expected domain.");
                    context.Fail();
                }
            }
            else
            {
                context.Succeed(requirement);

            
            }

            



        }
    }
}
