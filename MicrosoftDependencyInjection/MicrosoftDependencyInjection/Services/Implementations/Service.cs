namespace MicrosoftDependencyInjection.Services.Implementations;

public abstract class Service : IService
{
    public Guid Id { get; } = Guid.NewGuid();
}