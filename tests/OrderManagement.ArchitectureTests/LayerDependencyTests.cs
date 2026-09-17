using ArchUnitNET.Fluent;
using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace OrderManagement.ArchitectureTests;

public sealed class LayerDependencyTests : ArchitectureTestBase
{
    [Fact]
    public void Infrastructure_Should_Not_Depend_On_Api()
    {
        IArchRule rule = Types().That().Are(InfrastructureLayer).Should()
            .NotDependOnAny(ApiLayer)
            .Because("ADR-001 reserves composition responsibilities for the API");

        rule.Check(Architecture);
    }

    [Fact]
    public void Application_Should_Not_Depend_On_Adapters_Or_Api()
    {
        IArchRule rule = Types().That().Are(ApplicationLayer).Should()
            .NotDependOnAny(InfrastructureLayer)
            .AndShould().NotDependOnAny(ApiLayer)
            .Because("ADR-001 allows application code to depend inward only");

        rule.Check(Architecture);
    }

    [Fact]
    public void Domain_Should_Not_Depend_On_Outer_Layers()
    {
        IArchRule rule = Types().That().Are(DomainLayer).Should()
            .NotDependOnAny(ApplicationLayer)
            .AndShould().NotDependOnAny(InfrastructureLayer)
            .AndShould().NotDependOnAny(ApiLayer)
            .Because("ADR-001 keeps the domain independent of outer layers");

        rule.Check(Architecture);
    }
}