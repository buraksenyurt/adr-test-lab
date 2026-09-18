using System.Text.RegularExpressions;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace OrderManagement.ArchitectureTests;

public sealed class FrameworkIndependenceTests : ArchitectureTestBase
{
    private static string ForbiddenNamespaces(params string[] roots) =>
        "^(" + string.Join("|", roots.Select(Regex.Escape)) + @")(\..*)?$";

    private static readonly string ForbiddenInDomain = ForbiddenNamespaces(
        "Microsoft.AspNetCore",
        "Microsoft.Extensions",
        "Microsoft.EntityFrameworkCore",
        "System.Data",
        "System.Net.Http",
        "System.Text.Json",
        "Newtonsoft.Json");

    private static readonly string ForbiddenInApplication = ForbiddenNamespaces(
        "Microsoft.AspNetCore",
        "Microsoft.EntityFrameworkCore",
        "System.Data",
        "System.Net.Http");

    [Fact]
    public void Domain_Should_Not_Depend_On_Frameworks()
    {
        IArchRule rule = Types().That().Are(DomainLayer).Should()
            .NotDependOnAnyTypesThat()
            .ResideInNamespaceMatching(ForbiddenInDomain)
            .Because("ADR-005 keeps domain types free of serialization, DI and persistence frameworks");

        rule.Check(Architecture);
    }

    [Fact]
    public void Application_Should_Not_Depend_On_Web_Or_Orm_Frameworks()
    {
        IArchRule rule = Types().That().Are(ApplicationLayer).Should()
            .NotDependOnAnyTypesThat()
            .ResideInNamespaceMatching(ForbiddenInApplication)
            .Because("ADR-005 keeps use cases free of transport and ORM concerns");

        rule.Check(Architecture);
    }

    [Fact]
    public void Architecture_Should_See_External_Framework_Dependencies()
    {
        var externalTargets = Architecture.Types
            .Where(t => t.FullName.StartsWith("OrderManagement.Api"))
            .SelectMany(t => t.Dependencies)
            .Select(d => d.Target.FullName)
            .Where(n => n.StartsWith("Microsoft.AspNetCore"))
            .ToList();

        Assert.NotEmpty(externalTargets);
    }
}