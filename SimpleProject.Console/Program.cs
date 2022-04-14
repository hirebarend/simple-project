// See https://aka.ms/new-console-template for more information
using SimpleProject.Shared.Misc;

Console.WriteLine("Hello, World!");

var circuitBreaker = new CircuitBreaker();

for (var i = 0; i < 20; i++)
{
    try
    {
        var result = circuitBreaker.Execute(() =>
        {
            ChaosMonkey.Do();

            return Task.FromResult(0);

        }).GetAwaiter().GetResult();

        Console.WriteLine($"SUCCESS");
    }
    catch
    {
        Console.WriteLine($"FAILURE");
    }

    Thread.Sleep(1000);
}