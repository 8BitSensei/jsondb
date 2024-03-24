using JsonDb.Adapters;
using Octokit;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonDb.Github
{
    internal class GithubJsonCollection<T> : InMemoryJsonCollection<T>
    {
        private readonly GitHubClient _client;
        private readonly string _repoOwner;
        private readonly string _repoName;
        private readonly string _localRepoPath;
        private readonly string _branch;
        private readonly IJsonCollectionAdapter _collectionAdapter;
        private readonly JsonSerializerOptions _serializerOptions;

        public GithubJsonCollection(GitHubClient client, string repOwner, 
                                    string repoName, string localRepoPath, string branch,
                                    IJsonCollectionAdapter jsonCollectionAdapter, 
                                    JsonSerializerOptions jsonSerializerOptions)
        {
            _client = client;
            _repoOwner = repOwner;
            _repoName = repoName;
            _localRepoPath = localRepoPath;
            _branch = branch;
            _collectionAdapter = jsonCollectionAdapter;
            _serializerOptions = jsonSerializerOptions;
        }

        internal string Sha { get; set; }
        internal bool SingularObject { get; set; }

        protected override async Task WriteAsync(IEnumerable<T> items) 
        {
            string json;
            if (!SingularObject)
                json = JsonSerializer.Serialize(items, _serializerOptions);
            else
                json = JsonSerializer.Serialize(items.FirstOrDefault(), _serializerOptions);
            
            var utf8Buffer = Encoding.UTF8.GetBytes(json);
            if (_collectionAdapter != null)
                utf8Buffer = await _collectionAdapter.WriteAsync(utf8Buffer);

            using var stream = new MemoryStream(utf8Buffer);
                var results = _client.Repository.Content.UpdateFile(_repoOwner, _repoName, _localRepoPath, new UpdateFileRequest($"this is a test", json, Sha, _branch)).Result; // Strapi API gets an obejct not an array, this is an issue.. maybe just have to wrap it in an array. Maybe array of different collecitons..
        }

        
    }
}
