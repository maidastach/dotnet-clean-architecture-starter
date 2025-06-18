using Microsoft.Extensions.DependencyInjection;

namespace { SolutionName }.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class ServiceLifetimeAttribute(ServiceLifetime lifetime) : Attribute
{
    public ServiceLifetime Lifetime { get; } = lifetime;
}
