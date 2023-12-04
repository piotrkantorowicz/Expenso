using Expenso.Shared.ModuleDefinition;
using Expenso.Shared.ModuleDefinition.Extensions;

namespace Expenso.Shared.Tests.UnitTests.ModuleDefinition.Extensions.EndpointRegistrationExtensions.Cases;

internal sealed class WithLeadingSlash : EndpointRegistrationExtensionsTestBase
{
    [Test, TestCase(""), TestCase("/users")]
    public void Should_ReturnUnchangedValue_When_PatternIsEmpty_Or_PatternIsAlreadyValid(string pattern)
    {
        // Arrange
        CustomizeEndpointRegistration(pattern);

        // Act
        EndpointRegistration testResult = TestCandidate.WithLeadingSlash();

        // Assert
        testResult
            .Should()
            .Be(TestCandidate);
    }

    [Test, TestCase("users")]
    public void Should_ReturnSanitizedValue_When_PatternDoNotHaveLeadingSlash(string pattern)
    {
        // Arrange
        CustomizeEndpointRegistration(pattern);

        // Act
        EndpointRegistration testResult = TestCandidate.WithLeadingSlash();

        // Assert
        testResult
            .Pattern.Should()
            .Be($"/{TestCandidate.Pattern}");
    }
}