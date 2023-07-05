using Microsoft.AspNetCore.SignalR;

namespace SignalRTest.Hubs
{
    public class TestHub : Hub
    {
        public static int TotalCount { get; set; } = 0;

        public async Task NewCall()
        {
            TotalCount++;

            await Clients.All.SendAsync("updateTotalCount", TotalCount);
        }
    }
}
