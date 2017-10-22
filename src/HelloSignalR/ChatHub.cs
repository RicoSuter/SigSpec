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

        public Task Foo(Bar bar)
        {
            return Task.CompletedTask;
        }
    }

    public class Bar
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