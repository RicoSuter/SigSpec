using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace HelloSignalR
{
    public class ChatHub : Hub<IChatClient>
    {
        public Task Send(string message)
        {
            if (message == string.Empty)
            {
                return Clients.All.Welcome();
            }

            return Clients.All.Send(message);
        }

        public Task AddPerson(Person person)
        {
            return Task.CompletedTask;
        }

        public ChannelReader<Event> GetEvents()
        {
            var channel = Channel.CreateUnbounded<Event>();
            // TODO: Write events
            return channel.Reader;
        }
    }

    public class Event
    {
        public string Type { get; set; }
    }

    public class Person
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }
    }

    public interface IChatClient
    {
        Task Welcome();

        Task Send(string message);
    }
}