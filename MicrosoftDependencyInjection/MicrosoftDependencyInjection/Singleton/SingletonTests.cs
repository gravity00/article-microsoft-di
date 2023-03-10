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
            .AddSingleton<AggregatorService>()
            .BuildServiceProvider();

        var aggregatorService = provider.GetRequiredService<AggregatorService>();

        Assert.Equal(
            aggregatorService.Service1Id,
            aggregatorService.Service2Id
        );
    }

    [Fact]
    public void ManualInitialization()
    {
        IService service = new Service();

        var aggregatorService = new AggregatorService(
            service,
            service
        );

        Assert.Equal(
            aggregatorService.Service1Id,
            aggregatorService.Service2Id
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

public class AggregatorService
{
    private readonly IService _service1;
    private readonly IService _service2;

    public AggregatorService(IService service1, IService service2)
    {
        _service1 = service1;
        _service2 = service2;
    }

    public Guid Service1Id => _service1.Id;

    public Guid Service2Id => _service2.Id;
}