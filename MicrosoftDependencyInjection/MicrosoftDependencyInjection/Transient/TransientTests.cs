using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MicrosoftDependencyInjection.Transient;

public class TransientTests
{
    [Fact]
    public void ContainerInitialization()
    {
        using var provider = new ServiceCollection()
            .AddTransient<IService, Service>()
            .BuildServiceProvider();

        var service1 = provider.GetRequiredService<IService>();
        var service2 = provider.GetRequiredService<IService>();

        Assert.NotEqual(
            service1.Id,
            service2.Id
        );
    }

    [Fact]
    public void ManualInitialization()
    {
        IService service1 = new Service();
        IService service2 = new Service();

        Assert.NotEqual(
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