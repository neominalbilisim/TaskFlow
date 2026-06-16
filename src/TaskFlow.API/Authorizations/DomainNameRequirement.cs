using Microsoft.AspNetCore.Authorization;

namespace TaskFlow.API.Authorizations
{
    public class DomainNameRequirement: AuthorizeAttribute, IAuthorizationRequirement
    {
        public string RequiredDomain { get; init; }

        public DomainNameRequirement(string requiredDomain)
        {
            RequiredDomain = requiredDomain;
        }
    }
}
