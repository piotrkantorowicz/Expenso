using Expenso.Api.Tests.E2E.TestData.IAM;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.CreateBudgetPermission.DTO.Request;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.CreateBudgetPermission.DTO.Response;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissions;

[TestFixture]
internal sealed class CreateBudgetPermission : BudgetPermissionTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);
        const string requestPath = "budget-sharing/budget-permissions";
        Guid budgetPermissionId = Guid.NewGuid();
        Guid budgetId = Guid.NewGuid();

        CreateBudgetPermissionRequest createBudgetPermissionRequest = new(BudgetPermissionId: budgetPermissionId,
            BudgetId: budgetId, OwnerId: UserDataInitializer.UserIds[index: 3]);

        // Act
        HttpResponseMessage response =
            await _httpClient.PostAsJsonAsync(requestUri: requestPath, value: createBudgetPermissionRequest);

        // Assert
        AssertResponseCreated(response: response);

        CreateBudgetPermissionResponse? createBudgetPermissionResponse =
            await response.Content.ReadFromJsonAsync<CreateBudgetPermissionResponse>();

        createBudgetPermissionResponse?.BudgetPermissionId.Should().Be(expected: budgetPermissionId);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        const string requestPath = "budget-sharing/budget-permissions";

        // Act
        HttpResponseMessage response = await _httpClient.PostAsync(requestUri: requestPath, content: null);

        // Assert
        AssertResponseUnauthroised(response: response);
    }
}