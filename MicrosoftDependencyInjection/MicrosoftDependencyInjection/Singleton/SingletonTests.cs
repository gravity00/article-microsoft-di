using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MicrosoftDependencyInjection.Singleton;

public class SingletonTests
{
    [Fact]
    public void ContainerInitialization()
    {
        var provider = new ServiceCollection()
            .AddSingleton<ISingletonService, SingletonService>()
            .AddSingleton<TestService>()
            .BuildServiceProvider();

        var testService = provider.GetRequiredService<TestService>();

        Assert.Equal(
            testService.SingletonService1Id,
            testService.SingletonService2Id
        );
    }

    [Fact]
    public void ManualInitialization()
    {
        ISingletonService singletonService = new SingletonService();

        var testService = new TestService(
            singletonService,
            singletonService
        );

        Assert.Equal(
            testService.SingletonService1Id,
            testService.SingletonService2Id
        );
    }
}

public interface ISingletonService
{
    Guid Id { get; }
}

public class SingletonService : ISingletonService
{
    public Guid Id { get; } = Guid.NewGuid();
}

public class TestService
{
    private readonly ISingletonService _singletonService1;
    private readonly ISingletonService _singletonService2;

    public TestService(ISingletonService singletonService1, ISingletonService singletonService2)
    {
        _singletonService1 = singletonService1;
        _singletonService2 = singletonService2;
    }

    public Guid SingletonService1Id => _singletonService1.Id;

    public Guid SingletonService2Id => _singletonService2.Id;
}