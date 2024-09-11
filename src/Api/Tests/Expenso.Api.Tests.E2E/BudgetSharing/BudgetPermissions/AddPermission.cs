using Expenso.Api.Tests.E2E.TestData.BudgetSharing;
using Expenso.Api.Tests.E2E.TestData.IAM;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.AddPermission.DTO.Request;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissions;

internal sealed class AddPermission : BudgetPermissionTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);

        string requestPath =
            $"budget-sharing/budget-permissions/{BudgetPermissionDataInitializer.BudgetPermissionIds[index: 0]}/participants/{UserDataInitializer.UserIds[index: 4]}";

        AddPermissionRequest addPermissionRequest = new(PermissionType: AddPermissionRequest_PermissionType.Reviewer);

        // Act
        HttpResponseMessage response =
            await _httpClient.PostAsJsonAsync(requestUri: requestPath, value: addPermissionRequest);

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
        HttpResponseMessage response = await _httpClient.PostAsync(requestUri: requestPath, content: null);

        // Assert
        AssertResponseUnauthroised(response: response);
    }
}