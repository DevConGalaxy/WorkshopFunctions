using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LPI.Models;

namespace LPI.Services
{
    public class GithubService
    {
        private HttpClient httpClient;
        public GithubService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<string> SendToGithub(Meetup[] meetups)
        {
            var meetupsstring = JsonSerializer.Serialize(meetups);
            string base64Encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(meetupsstring));
            httpClient.DefaultRequestHeaders.Add("User-Agent","insertyourusernamehere");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Token insertyourtokenhere");
            var returnGithub = await httpClient.GetAsync("https://api.github.com/repos/DevConGalaxy/DevConGalaxy.github.io/contents/src/assets/meetupsdata.json");
            GithubCommit githubCommit;
            if(returnGithub.StatusCode == System.Net.HttpStatusCode.NotFound) {
                githubCommit = new GithubCommit("insertyourusernamehere","insertyouremailhere","Adding meetupsData.json",base64Encoded);
            } else {
                var githubResultSearchString = await returnGithub.Content.ReadAsStringAsync();
                var githubResultSearch = JsonSerializer.Deserialize<GithubResultSearch>(githubResultSearchString);
                githubCommit = new GithubCommit("insertyourusernamehere","insertyouremailhere","Updating meetupsData.json",base64Encoded,sha: githubResultSearch.sha);
            }

            var stringCommit = JsonSerializer.Serialize(githubCommit);
                var stringContent = new StringContent(stringCommit);
                var resultAddFile = await httpClient.PutAsync("https://api.github.com/repos/DevConGalaxy/DevConGalaxy.github.io/contents/src/assets/meetupsdata.json",stringContent);
            
            return base64Encoded;
        }
        public async Task<Meetup[]> RetrieveMeetupsList() {
            var meetupsHttpResult = await httpClient.GetAsync("https://raw.githubusercontent.com/DevConGalaxy/DevConGalaxy.github.io/master/src/assets/meetups.json");
            var meetupString = await meetupsHttpResult.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Result>(meetupString);
            return result.meetups;
        }
    }
}