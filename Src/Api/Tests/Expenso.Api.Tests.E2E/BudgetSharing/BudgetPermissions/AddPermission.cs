using Expenso.Api.Tests.E2E.TestData.BudgetSharing;
using Expenso.Api.Tests.E2E.TestData.Preferences;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.AddPermission.DTO.Request;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissions;

internal sealed class AddPermission : BudgetPermissionTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(_claims);

        string requestPath =
            $"budget-sharing/budget-permissions/{BudgetPermissionDataProvider.BudgetPermissionIds[0]}/participants/{PreferencesDataProvider.UserIds[4]}";

        AddPermissionRequest addPermissionRequest = new(AddPermissionRequest_PermissionType.Reviewer);

        // Act
        HttpResponseMessage testResult = await _httpClient.PostAsJsonAsync(requestPath, addPermissionRequest);

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
        HttpResponseMessage testResult = await _httpClient.PostAsync(requestPath, null);

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}