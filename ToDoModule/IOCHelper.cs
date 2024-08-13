using FeatureAuth;
using Microsoft.Extensions.DependencyInjection;

namespace ToDoModule;

public static class IOCHelper
{
    public static void AddToDoModule(this IServiceCollection services)
    {
        services.AddFeatureAuthDetails<FolderListAuth>();
    }
}
