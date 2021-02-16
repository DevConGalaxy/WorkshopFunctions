using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using LPI.Models;

namespace LPI.Services
{
    public class MeetupService
    {
        private HttpClient httpClient;
        public MeetupService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<Meetup[]> RetrieveLastMeetups(Meetup[] meetups)
        {
            for (int i = 0; i < meetups.Length; i++)
            {
                var item = meetups[i];
                var dataMeetup = await httpClient.GetAsync(item.url + "/events/json/");
                var dataMeetupString = await dataMeetup.Content.ReadAsStringAsync();
                try
                {
                    item.Events = JsonSerializer.Deserialize<Event[]>(dataMeetupString);
                }
                catch (System.Exception e)
                {}
            }
            
            return meetups;
        }
    }
}