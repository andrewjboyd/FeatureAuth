using Microsoft.AspNetCore.Authorization;

namespace FeatureAuth;

public class EndpointIdAttribute<T>(T endPointIdentifier) : AuthorizeAttribute, IAuthorizationRequirement, IAuthorizationRequirementData
    where T : Enum
{
    public T EndPointIdentifier { get; } = endPointIdentifier;

    public IEnumerable<IAuthorizationRequirement> GetRequirements()
    {
        yield return this;
    }
}
