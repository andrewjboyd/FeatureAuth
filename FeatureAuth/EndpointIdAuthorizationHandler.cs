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
        _logger.LogInformation("Evaluating authorization requirement for EndPointId = '{EndPointId}'", requirement.EndPointIdentifier);

        var authClaim = context.User.FindFirst(c => c.Type.Equals(_claimType, StringComparison.OrdinalIgnoreCase));
        if (authClaim is null)
        {
            return Task.CompletedTask;
        }

        if (!int.TryParse(authClaim.Value, out int endPointPermissions))
        {
            _logger.LogError("Could not convert '{AuthCliamValue}' to an integer for claim {authClaimType}", authClaim.Value, authClaim.Type);
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
