using Expenso.Shared.System.Modules;
using Expenso.Shared.System.Modules.Extensions;

namespace Expenso.Shared.Tests.UnitTests.System.Modules.Extensions.EndpointRegistrationExtensions;

internal sealed class WithLeadingSlash : EndpointRegistrationExtensionsTestBase
{
    [Test, TestCase(arg: ""), TestCase(arg: "/users")]
    public void Should_ReturnUnchangedValue_When_PatternIsEmpty_Or_PatternIsAlreadyValid(string pattern)
    {
        // Arrange
        CustomizeEndpointRegistration(pattern: pattern);

        // Act
        EndpointRegistration testResult = TestCandidate.WithLeadingSlash();

        // Assert
        testResult.Should().Be(expected: TestCandidate);
    }

    [Test, TestCase(arg: "users")]
    public void Should_ReturnSanitizedValue_When_PatternDoNotHaveLeadingSlash(string pattern)
    {
        // Arrange
        CustomizeEndpointRegistration(pattern: pattern);

        // Act
        EndpointRegistration testResult = TestCandidate.WithLeadingSlash();

        // Assert
        testResult.Pattern.Should().Be(expected: $"/{TestCandidate.Pattern}");
    }
}