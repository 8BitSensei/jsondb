using Microsoft.Extensions.Options;
using Octokit;

namespace JsonDb.Github
{
    internal class GithubJsonDbFactory : IJsonDbFactory
    {
        private readonly GithubJsonDbOptions _options;
        public GithubJsonDbFactory(IOptions<GithubJsonDbOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }

        public IJsonDb GetJsonDb()
        {
            var client = new GitHubClient(new ProductHeaderValue("GithubJsonDb"));
            client.Credentials = new Credentials(_options.PersonalAccessToken);
            return new GithubJsonDb(client, _options.RepoOwner, _options.RepoName, _options.FilePath, _options.Branch,
                                    _options.CollectionAdapter, _options.JsonSerializerOptions);
        }
    }
}