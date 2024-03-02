using Expenso.Api.Tests.E2E.TestData.BudgetSharing;
using Expenso.Api.Tests.E2E.TestData.Preferences;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissions;

internal sealed class RemovePermission : BudgetPermissionTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(_claims);

        string requestPath =
            $"budget-sharing/budget-permissions/{BudgetPermissionDataProvider.BudgetPermissionIds[0]}/participants/{PreferencesDataProvider.UserIds[3]}";

        // Act
        HttpResponseMessage testResult = await _httpClient.DeleteAsync(requestPath);

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        string requestPath =
            $"budget-sharing/budget-permissions/{BudgetPermissionDataProvider.BudgetPermissionIds[0]}/participants/{PreferencesDataProvider.UserIds[3]}";

        // Act
        HttpResponseMessage testResult = await _httpClient.DeleteAsync(requestPath);

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}