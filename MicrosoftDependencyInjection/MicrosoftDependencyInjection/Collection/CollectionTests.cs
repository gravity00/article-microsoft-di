using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MicrosoftDependencyInjection.Collection;

public class CollectionTests
{
    [Fact]
    public void ContainerInitialization()
    {
        using var provider = new ServiceCollection()
            .AddSingleton<IService, ServiceA>()
            .AddSingleton<IService, ServiceB>()
            .BuildServiceProvider();

        var services = provider.GetServices<IService>();
        
        Assert.Collection(services,
            item => Assert.IsType<ServiceA>(item),
            item => Assert.IsType<ServiceB>(item)
        );
    }

    [Fact]
    public void ManualInitialization()
    {
        var services = new IService[]
        {
            new ServiceA(),
            new ServiceB()
        };
        
        Assert.Collection(services,
            item => Assert.IsType<ServiceA>(item),
            item => Assert.IsType<ServiceB>(item)
        );
    }
}

public interface IService
{
    Guid Id { get; }
}

public class ServiceA : IService
{
    public Guid Id { get; } = Guid.NewGuid();
}

public class ServiceB : IService
{
    public Guid Id { get; } = Guid.NewGuid();
}