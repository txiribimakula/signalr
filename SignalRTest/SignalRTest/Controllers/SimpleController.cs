using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRTest.Hubs;

namespace SignalRTest.Controllers
{
    public class SimpleController : Controller
    {
        IHubContext<TestHub> hubContext;

        public SimpleController(IHubContext<TestHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        [HttpGet("simple")]
        public Task GetSimpleAsync()
        {
            Console.WriteLine("simple called");
            return hubContext.Clients.All.SendAsync("NewCall", "message");
        }
    }
}
