using System.Net.Http;
using LPI.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace LPI.Functions
{
    public static class GetMeetupsEventsTime
    {
        private static HttpClient _httpClient = new HttpClient();
        private static GithubService _githubService = new GithubService(_httpClient);
        private static MeetupService _meetupService = new MeetupService(_httpClient);
        [FunctionName("GetMeetupsEventsTime")]
        public static async void Run([TimerTrigger("0 0 0 * * *")]TimerInfo myTimer, ILogger log)
        {
            var meetups = await _githubService.RetrieveMeetupsList();
            var updatedMeetups = await _meetupService.RetrieveLastMeetups(meetups);
            var githubStatus = await _githubService.SendToGithub(updatedMeetups);
            log.LogInformation(githubStatus);
        }

    }
}
