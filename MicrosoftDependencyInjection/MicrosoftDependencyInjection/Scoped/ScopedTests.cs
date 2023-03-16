using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MicrosoftDependencyInjection.Scoped;

public class ScopedTests
{
    [Fact]
    public void ContainerInitialization()
    {
        using var provider = new ServiceCollection()
            .AddScoped<IService, Service>()
            .BuildServiceProvider(new ServiceProviderOptions
            {
                ValidateScopes = true
            });

        IService firstScopeService1;
        IService firstScopeService2;

        IService secondScopeService1;
        IService secondScopeService2;
        
        using (var firstScope = provider.CreateScope())
        {
            firstScopeService1 = firstScope.ServiceProvider.GetRequiredService<IService>();
            firstScopeService2 = firstScope.ServiceProvider.GetRequiredService<IService>();

            Assert.Equal(
                firstScopeService1.Id,
                firstScopeService2.Id
            );
        }

        using (var secondScope = provider.CreateScope())
        {
            secondScopeService1 = secondScope.ServiceProvider.GetRequiredService<IService>();
            secondScopeService2 = secondScope.ServiceProvider.GetRequiredService<IService>();

            Assert.Equal(
                secondScopeService1.Id,
                secondScopeService2.Id
            );
        }

        // compare different scopes
        Assert.NotEqual(
            firstScopeService1.Id,
            secondScopeService1.Id
        );

        Assert.NotEqual(
            firstScopeService2.Id,
            secondScopeService2.Id
        );
    }

    [Fact]
    public void ManualInitialization()
    {
        IService firstScopeService1;
        IService firstScopeService2;

        IService secondScopeService1;
        IService secondScopeService2;

        // simulate first scope
        {
            firstScopeService1 = firstScopeService2 = new Service();

            Assert.Equal(
                firstScopeService1.Id,
                firstScopeService2.Id
            );
        }

        // simulate second scope
        {
            secondScopeService1 = secondScopeService2 = new Service();

            Assert.Equal(
                secondScopeService1.Id,
                secondScopeService2.Id
            );
        }

        // compare different scopes
        Assert.NotEqual(
            firstScopeService1.Id,
            secondScopeService1.Id
        );

        Assert.NotEqual(
            firstScopeService2.Id,
            secondScopeService2.Id
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