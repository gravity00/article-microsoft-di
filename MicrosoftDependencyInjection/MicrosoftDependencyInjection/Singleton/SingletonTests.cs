using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MicrosoftDependencyInjection.Singleton;

public class SingletonTests
{
    [Fact]
    public void ContainerInitialization()
    {
        var provider = new ServiceCollection()
            .AddSingleton<IService, Service>()
            .BuildServiceProvider();

        var service1 = provider.GetRequiredService<IService>();
        var service2 = provider.GetRequiredService<IService>();

        Assert.Equal(
            service1.Id,
            service2.Id
        );
    }

    [Fact]
    public void ManualInitialization()
    {
        IService service1;
        IService service2;
        service1 = service2 = new Service();

        Assert.Equal(
            service1.Id,
            service2.Id
        );
    }
}

public interface IService
{
    Guid Id { get; }
}

public class Service : IService
{
    public Guid Id { get; } = Guid.NewGuid();
}