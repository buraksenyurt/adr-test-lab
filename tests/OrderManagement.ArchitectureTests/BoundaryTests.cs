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
}