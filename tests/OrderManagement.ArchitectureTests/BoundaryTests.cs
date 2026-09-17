using ArchUnitNET.Fluent;
using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace OrderManagement.ArchitectureTests;

public sealed class BoundaryTests : ArchitectureTestBase
{
    [Fact]
    public void Api_Should_Not_Depend_On_Persistence_Adapters()
    {
        var persistenceAdapters = Types().That()
            .ResideInNamespace("OrderManagement.Infrastructure.Persistence")
            .As("Persistence Adapters");

        IArchRule rule = Types().That().Are(ApiLayer).Should()
            .NotDependOnAny(persistenceAdapters)
            .Because("ADR-002 requires the API to use application ports instead of concrete adapters");

        rule.Check(Architecture);
    }

    [Fact]
    public void Api_Requests_Should_Reside_In_Contracts_Namespace()
    {
        IArchRule rule = Classes().That()
            .HaveNameContaining("Request")
            .Should()
            .ResideInNamespace("OrderManagement.Api.Contracts")
            .Because("ADR-003 keeps HTTP contracts at the API boundary");

        rule.Check(Architecture);
    }
}