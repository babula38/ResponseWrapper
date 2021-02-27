using ResponseWrapper.AspnetCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiResponseWrapper(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ResultBuilder>();

            return serviceCollection;
        }
    }
}
