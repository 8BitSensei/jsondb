using JsonDb;
using JsonDb.Github;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class GithubJsonDbFactoryServiceExtension
    {
        public static IServiceCollection AddGithubJsonDb(this IServiceCollection services, Action<GithubJsonDbOptions> configure) 
        {
            services.AddSingleton<IJsonDbFactory, GithubJsonDbFactory>()
                .Configure(configure);

            return services;
        }
    }
}
