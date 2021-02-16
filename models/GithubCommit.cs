namespace LPI.Models
{
    public class GithubCommit
    {
        public Comitter comitter { get; set; }
        public string message { get; set; }
        public string content { get; set; }
        public string sha { get; set; }

        public GithubCommit(string comitterName, string comitterEmail, string message, string content, string sha = null)
        {
            this.comitter = new Comitter{
                email = comitterEmail,
                name = comitterName
            };
            this.message = message;
            this.content = content;
            this.sha = sha;
        }
    }
}