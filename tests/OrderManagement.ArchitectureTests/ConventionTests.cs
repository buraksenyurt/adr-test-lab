using ArchUnitNET.Fluent;
using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace OrderManagement.ArchitectureTests;

public sealed class ConventionTests : ArchitectureTestBase
{
    [Fact]
    public void Application_Ports_Should_Be_Repository_Interfaces()
    {
        IArchRule rule = Interfaces().That()
            .ResideInNamespace("OrderManagement.Application.Ports")
            .Should()
            .HaveNameContaining("Repository")
            .Because("ADR-004 makes repository ports recognizable by name");

        rule.Check(Architecture);
    }

    [Fact]
    public void Persistence_Adapters_Should_Have_Repository_In_Their_Name()
    {
        IArchRule rule = Classes().That()
            .ResideInNamespace("OrderManagement.Infrastructure.Persistence")
            .Should()
            .HaveNameContaining("Repository")
            .Because("ADR-004 makes repository adapters recognizable by name");

        rule.Check(Architecture);
    }

    [Fact]
    public void Application_Order_Handlers_Should_Have_Handler_In_Their_Name()
    {
        IArchRule rule = Classes().That()
            .ResideInNamespace("OrderManagement.Application.Orders.Handlers")
            .Should()
            .HaveNameContaining("Handler")
            .Because("ADR-004 gives application handlers a consistent suffix");

        rule.Check(Architecture);
    }
}