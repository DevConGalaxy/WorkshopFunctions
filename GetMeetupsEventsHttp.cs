using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using LPI.Services;

namespace Company.Functions
{
    public static class GetMeetupsEventsHttp
    {
        private static HttpClient _httpClient = new HttpClient();
        private static GithubService _githubService = new GithubService(_httpClient);
        private static MeetupService _meetupService = new MeetupService(_httpClient);

        [FunctionName("GetMeetupsEventsHttp")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var meetups = await _githubService.RetrieveMeetupsList();
            var updatedMeetups = await _meetupService.RetrieveLastMeetups(meetups);
            var githubStatus = await _githubService.SendToGithub(updatedMeetups);
            log.LogInformation(githubStatus);
            return new OkObjectResult(githubStatus);
        }
    }
}
