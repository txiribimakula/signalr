// See https://aka.ms/new-console-template for more information
using System;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Net.Http;

Console.WriteLine("Hello, World!");

using (var client = new HttpClient())
{
    using (var stream = await client.GetStreamAsync("https://localhost:7091/weatherforecast/sse"))
    {
        using (var reader = new StreamReader(stream))
        {
            while (!reader.EndOfStream)
            {
                var text = await reader.ReadLineAsync();
                Console.WriteLine(text);
            }
        }
    }
}