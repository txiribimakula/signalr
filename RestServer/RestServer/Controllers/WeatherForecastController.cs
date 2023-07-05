using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection.PortableExecutable;
using System.Text.Json.Serialization;
using System.Threading.Channels;

namespace RestServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ChannelWriter<Message> _channelWriter;
        private readonly ChannelReader<Message> _channelReader;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            ChannelWriter<Message> channelWriter,
            ChannelReader<Message> channelReader)
        {
            _logger = logger;
            _channelWriter = channelWriter;
            _channelReader = channelReader;
        }

        [HttpGet("SSE")]
        public async Task GetSseAsync()
        {
            Response.Headers.Add("Content-Type", "application/x-ndjson");

            //for (var i = 0; i < 2; ++i)
            //{
            //    await response
            //        .WriteAsJsonAsync(new TestData());
            //        // .WriteAsync($"data: Controller {i} at {DateTime.Now}\r\r");

            //    await Task.Delay(5 * 1000);
            //}

            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            _ = Task.Run(async () =>
            {
                while (true)
                {
                    await Response.WriteAsync(JsonConvert.SerializeObject(new Message() { Data = "WIP" }) + "\n");
                    await Task.Delay(10 * 1000);
                }
            }, token);

            _ = Task.Run(async () =>
            {
                while (await _channelReader.WaitToReadAsync(token))
                {
                    if (_channelReader.TryRead(out var msg))
                    {
                        Console.WriteLine(msg.Data);
                        await Response.WriteAsync(JsonConvert.SerializeObject(msg) + "\n");
                    }
                }
            }, token);

            await Task.Delay(30 * 1000);

            cts.Cancel();

            await Response.Body.FlushAsync();
        }

        [HttpPost("write")]
        public async Task WriteAsync(Message msg)
        {
            await _channelWriter.WriteAsync(msg);
        }

    }
}