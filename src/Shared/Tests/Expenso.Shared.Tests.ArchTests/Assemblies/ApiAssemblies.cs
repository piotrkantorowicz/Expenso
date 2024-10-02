using Expenso.Shared.Api.ProblemDetails.Details;
using Expenso.Shared.Api.Swagger;

namespace Expenso.Shared.Tests.ArchTests.Assemblies;

internal static class ApiAssemblies
{
    private static readonly Assembly ProblemDetails = typeof(BaseTypedProblemDetails<>).Assembly;
    private static readonly Assembly Swagger = typeof(ISchemaDescriptor).Assembly;

    private static readonly Dictionary<string, Assembly> Assemblies = new()
    {
        [key: nameof(ProblemDetails)] = ProblemDetails,
        [key: nameof(Swagger)] = Swagger
    };

    public static IReadOnlyDictionary<string, Assembly> GetAssemblies()
    {
        return Assemblies;
    }
}