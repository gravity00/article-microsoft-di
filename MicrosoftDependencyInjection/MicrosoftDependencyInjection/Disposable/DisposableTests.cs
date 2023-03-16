using Microsoft.Extensions.DependencyInjection;
using Xunit;
#pragma warning disable CA1816

namespace MicrosoftDependencyInjection.Disposable;

public class DisposableTests
{
    [Fact]
    public void ContainerInitialization()
    {
        SingletonService singletonService;
        TransientService transientOuterService;

        using (var provider = new ServiceCollection()
                   .AddSingleton<SingletonService>()
                   .AddScoped<ScopedService>()
                   .AddTransient<TransientService>()
                   .BuildServiceProvider(new ServiceProviderOptions
                   {
                       ValidateScopes = true
                   })
              )
        {
            singletonService = provider.GetRequiredService<SingletonService>();
            transientOuterService = provider.GetRequiredService<TransientService>();

            Assert.False(singletonService.Disposed);
            Assert.False(transientOuterService.Disposed);

            ScopedService scopedService;
            TransientService transientInnerService;

            using (var scope = provider.CreateScope())
            {
                scopedService = scope.ServiceProvider.GetRequiredService<ScopedService>();
                transientInnerService = scope.ServiceProvider.GetRequiredService<TransientService>();

                Assert.False(scopedService.Disposed);
                Assert.False(transientInnerService.Disposed);
            }

            Assert.True(scopedService.Disposed);
            Assert.True(transientInnerService.Disposed);
        }

        Assert.True(singletonService.Disposed);
        Assert.True(transientOuterService.Disposed);
    }

    [Fact]
    public void ManualInitialization()
    {
        SingletonService singletonService;
        TransientService transientOuterService;

        using (singletonService = new SingletonService())
        using (transientOuterService = new TransientService())
        {
            Assert.False(singletonService.Disposed);
            Assert.False(transientOuterService.Disposed);

            ScopedService scopedService;
            TransientService transientInnerService;

            // simulate scope
            using (scopedService = new ScopedService())
            using (transientInnerService = new TransientService())
            {
                Assert.False(scopedService.Disposed);
                Assert.False(transientInnerService.Disposed);
            }

            Assert.True(scopedService.Disposed);
            Assert.True(transientInnerService.Disposed);
        }

        Assert.True(singletonService.Disposed);
        Assert.True(transientOuterService.Disposed);
    }
}

public class SingletonService : IDisposable
{
    public bool Disposed { get; private set; }

    public void Dispose() => Disposed = true;
}

public class ScopedService : IDisposable
{
    public bool Disposed { get; private set; }

    public void Dispose() => Disposed = true;
}

public class TransientService : IDisposable
{
    public bool Disposed { get; private set; }

    public void Dispose() => Disposed = true;
}