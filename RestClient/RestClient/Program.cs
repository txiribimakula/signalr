// See https://aka.ms/new-console-template for more information
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;

Console.WriteLine("Hello, World!");

using HttpClient client = new();

using (var streamReader = new StreamReader(await client.GetStreamAsync("https://localhost:7091/weatherforecast/sse")))
{
    while (!streamReader.EndOfStream)
    {
        var message = await streamReader.ReadLineAsync();
        Console.WriteLine($"Received message: {message}");
    }
}