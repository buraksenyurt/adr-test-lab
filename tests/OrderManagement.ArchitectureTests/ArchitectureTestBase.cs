using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace OrderManagement.ArchitectureTests;

public abstract class ArchitectureTestBase
{
    protected static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(
            typeof(Domain.IDomainAssemblyMarker).Assembly,
            typeof(Application.IApplicationAssemblyMarker).Assembly,
            typeof(Infrastructure.IInfrastructureAssemblyMarker).Assembly,
            typeof(Api.IApiAssemblyMarker).Assembly)
        .Build();

    protected static readonly IObjectProvider<IType> DomainLayer = Types()
        .That()
        .ResideInAssembly(typeof(Domain.IDomainAssemblyMarker).Assembly)
        .As("Domain Layer");

    protected static readonly IObjectProvider<IType> ApplicationLayer = Types()
        .That()
        .ResideInAssembly(typeof(Application.IApplicationAssemblyMarker).Assembly)
        .As("Application Layer");

    protected static readonly IObjectProvider<IType> InfrastructureLayer = Types()
        .That()
        .ResideInAssembly(typeof(Infrastructure.IInfrastructureAssemblyMarker).Assembly)
        .As("Infrastructure Layer");

    protected static readonly IObjectProvider<IType> ApiLayer = Types()
        .That()
        .ResideInAssembly(typeof(Api.IApiAssemblyMarker).Assembly)
        .As("API Layer");
}