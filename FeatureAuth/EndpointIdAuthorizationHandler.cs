using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace FeatureAuth;

internal class EndpointIdAuthorizationHandler<T>(ILogger<EndpointIdAuthorizationHandler<T>> logger)
    : AuthorizationHandler<EndpointIdAttribute<T>>
    where T : Enum
{
    private readonly ILogger<EndpointIdAuthorizationHandler<T>> _logger = logger;
    private readonly string _claimType = FeatureAuthHelpers.GetClaimType<T>();

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EndpointIdAttribute<T> requirement)
    {
        // Log as a warning so that it's very clear in sample output which authorization
        // policies(and requirements/handlers) are in use.
        _logger.LogInformation("Evaluating authorization requirement for EndPointId = '{EndPointId}'", requirement.EndPointIdentifier);

        var authClaim = context.User.FindFirst(c => c.Type == _claimType);
        if (authClaim is null)
        {
            return Task.CompletedTask;
        }

        if (!int.TryParse(authClaim.Value, out int endPointPermissions))
        {
            _logger.LogError("Could not convert '{AuthCliamValue}' to an integer for cliam {authClaimType}", authClaim.Value, authClaim.Type);
            return Task.CompletedTask;
        }

        var endPointIdentifier = (int)(object)requirement.EndPointIdentifier;

        if ((endPointPermissions & endPointIdentifier) != 0)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }
}
