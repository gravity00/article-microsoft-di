using Microsoft.Extensions.DependencyInjection;
using MicrosoftDependencyInjection.Services;
using MicrosoftDependencyInjection.Services.Implementations;

var services = new ServiceCollection();

services.AddSingleton<ISingletonService, SingletonService>();
services.AddScoped<IScopedService, ScopedService>();
services.AddTransient<ITransientService, TransientService>();

var provider = services.BuildServiceProvider(new ServiceProviderOptions
{
    ValidateOnBuild = true,
    ValidateScopes = true
});

var singleton = provider.GetRequiredService<ISingletonService>();
Console.WriteLine($"SingletonService.Id: {singleton.Id}");

var transient = provider.GetRequiredService<ITransientService>();
Console.WriteLine($"TransientService.Id: {transient.Id}");

await using var scope = provider.CreateAsyncScope();

var scoped = scope.ServiceProvider.GetRequiredService<IScopedService>();
Console.WriteLine($"ScopedService.Id: {scoped.Id}");

Console.WriteLine("Application terminated. Press <enter> to exit...");
Console.ReadLine();