namespace SamplePollyRefit.Services.GitHubApi.Response
{
    public class RepoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Private { get; set; }
        public int Size { get; set; }
    }
}
