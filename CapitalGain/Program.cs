using CapitalGain;
using CapitalGain.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{
    private static void Main(string[] args)
    {
        var services = new ServiceCollection();

        IConfiguration Config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

        services.AddTransient(provider =>
        {
            var settings = new ApplicationSettings();
            Config.GetSection("ApplicationSettings").Bind(settings);
            return new ConsoleApp(settings);
        });
        
        services.BuildServiceProvider()
            .GetService<ConsoleApp>()!.Run();        
    }
}