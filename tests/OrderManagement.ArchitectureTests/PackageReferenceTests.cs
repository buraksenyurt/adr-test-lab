using System.Xml.Linq;

namespace OrderManagement.ArchitectureTests;

public sealed class PackageReferenceTests
{
    private static readonly string[] ForbiddenPackagePrefixes =
    [
        "Microsoft.EntityFrameworkCore",
        "Microsoft.AspNetCore",
        "Microsoft.Data.SqlClient",
        "System.Data.SqlClient",
        "Dapper",
        "Newtonsoft.Json"
    ];

    public static TheoryData<string> GuardedProjects =>
    [
        "src/OrderManagement.Domain/OrderManagement.Domain.csproj",
        "src/OrderManagement.Application/OrderManagement.Application.csproj"
    ];

    [Theory]
    [MemberData(nameof(GuardedProjects))]
    public void Inner_Layers_Should_Not_Reference_Infrastructure_Packages(string relativePath)
    {
        var projectFile = Path.Combine(
            RepositoryRoot(),
            relativePath.Replace('/', Path.DirectorySeparatorChar));

        Assert.True(File.Exists(projectFile), $"Project file not found: {projectFile}");

        var referencedPackages = XDocument.Load(projectFile)
            .Descendants("PackageReference")
            .Select(element =>
                (string?)element.Attribute("Include") ?? (string?)element.Attribute("Update"))
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Select(name => name!)
            .ToList();

        var violations = referencedPackages
            .Where(package => ForbiddenPackagePrefixes.Any(forbidden =>
                package.Equals(forbidden, StringComparison.OrdinalIgnoreCase) ||
                package.StartsWith(forbidden + ".", StringComparison.OrdinalIgnoreCase)))
            .ToList();

        Assert.True(
            violations.Count == 0,
            $"ADR-005 violation: {relativePath} is referencing forbidden packages:\n  "
            + string.Join("\n  ", violations));
    }

    private static string RepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null &&
               !File.Exists(Path.Combine(directory.FullName, "OrderManagement.slnx")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName
            ?? throw new InvalidOperationException(
                "OrderManagement.slnx not found; repository root could not be determined.");
    }
}