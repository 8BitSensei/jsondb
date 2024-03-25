using JsonDb.Adapters;
using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JsonDb.Github
{
    public class GithubJsonDb : IJsonDb
    {
        private readonly GitHubClient _client;
        private readonly string _repoOwner;
        private readonly string _repoName;
        private readonly string _filePath;
        private readonly string _branch;
        private readonly IJsonCollectionAdapter _collectionAdapter;
        private readonly JsonSerializerOptions _serializerOptions;

        public GithubJsonDb(GitHubClient client, string repOwner,
                            string repoName, string filePath, string branch,
                            IJsonCollectionAdapter jsonCollectionAdapter,
                            JsonSerializerOptions jsonSerializerOptions) 
        {
            _client = client;
            _repoOwner = repOwner;
            _repoName = repoName;
            _filePath = filePath;
            _branch = branch;
            _collectionAdapter = jsonCollectionAdapter;
            _serializerOptions = jsonSerializerOptions;
        }

        public async Task<IJsonCollection<T>> GetCollectionAsync<T>(string name)
        {
            var collection = new GithubJsonCollection<T>(_client, _repoOwner, _repoName, $"{_filePath}/{name}.json",
                                                        _branch, _collectionAdapter, _serializerOptions);
            try 
            {
                var repoContents = _client.Repository.Content.GetAllContentsByRef(_repoOwner, _repoName, $"{_filePath}/{name}.json", _branch).Result;
                if (repoContents is null)
                    return collection;

                if (repoContents.Any())
                {
                    var fileContents = repoContents.FirstOrDefault()?.Content;
                    if (fileContents == null || string.IsNullOrEmpty(fileContents))
                        return collection;

                    using var ms = new MemoryStream(Encoding.UTF8.GetBytes(fileContents));
                    var content = await JsonSerializer.DeserializeAsync<T>(ms);
                    if (content is not null) 
                    {
                        if (content is IEnumerable<T>)
                            foreach (var item in (IEnumerable<T>)content)
                                collection.Add(content);
                        else
                        {
                            collection.Add(content);
                            collection.SingularObject = true;
                        }
                    }

                    collection.Sha = repoContents.First().Sha; // Hmm, not great
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);    
            }

            return collection;
        }

        
    }
}
