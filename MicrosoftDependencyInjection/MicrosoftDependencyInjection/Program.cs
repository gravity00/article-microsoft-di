using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

var provider = services.BuildServiceProvider(new ServiceProviderOptions
{
    ValidateOnBuild = true,
    ValidateScopes = true
});

Console.WriteLine("Application terminated. Press <enter> to exit...");
Console.ReadLine();
