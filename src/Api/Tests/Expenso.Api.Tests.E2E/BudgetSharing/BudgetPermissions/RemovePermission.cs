using Expenso.Api.Tests.E2E.TestData.BudgetSharing;
using Expenso.Api.Tests.E2E.TestData.IAM;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissions;

internal sealed class RemovePermission : BudgetPermissionTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);

        string requestPath =
            $"budget-sharing/budget-permissions/{BudgetPermissionDataInitializer.BudgetPermissionIds[index: 0]}/participants/{UserDataInitializer.UserIds[index: 3]}";

        // Act
        HttpResponseMessage response = await _httpClient.DeleteAsync(requestUri: requestPath);

        // Assert
        AssertResponseNoContent(response: response);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        string requestPath =
            $"budget-sharing/budget-permissions/{BudgetPermissionDataInitializer.BudgetPermissionIds[index: 0]}/participants/{UserDataInitializer.UserIds[index: 3]}";

        // Act
        HttpResponseMessage response = await _httpClient.DeleteAsync(requestUri: requestPath);

        // Assert
        AssertResponseUnauthroised(response: response);
    }
}