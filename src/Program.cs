// See https://aka.ms/new-console-template for more information

namespace Citcord;

public static class Program
{
    public static async Task Main()
    {
        await RunAsync();
    }

    private static async Task RunAsync()
    {
        var client = new CitcordClient();

        await client.LoginAsync();

        await Task.Delay(-1);
    }
}