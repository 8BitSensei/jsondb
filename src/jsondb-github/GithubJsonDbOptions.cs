namespace JsonDb.Github
{
    public class GithubJsonDbOptions : JsonDbOptions
    {
        public string RepoOwner { get; set; }
        public string RepoName { get; set; }
        public string FilePath { get; set; }
        public string PersonalAccessToken { get; set; }
        public string Branch { get; set; } = "main";
    }
}
