using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FeatureAuth;
public static class IOCHelpers
{
    public static void AddFlexibleAuthDetails<T>(this IServiceCollection services)
        where T : struct, Enum
    {
        if (EnumFibonacciValidator.IsValid<T>())
        {
            services.AddSingleton<IAuthorizationHandler, EndpointIdAuthorizationHandler<T>>();
            services.AddSingleton<IFeatureAuthDetails, FeatureAuthDetails<T>>();
        }
        else
        {
            throw new Exception($"Invalid Fibonacci range for '{typeof(T).FullName}'.  Values must be greater than 0 and you can't skip values (ie 1, 3, 5 where 2 is missing)");
        }

        // We only ever want one instance, so use TryAddSingleton
        services.TryAddSingleton<IFeatureAuthRepository, FeatureAuthRepository>();
    }
}
