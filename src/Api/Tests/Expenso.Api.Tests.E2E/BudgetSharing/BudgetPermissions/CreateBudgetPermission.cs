using Expenso.Api.Tests.E2E.TestData.IAM;
using Expenso.BudgetSharing.Proxy.DTO.API.CreateBudgetPermission.Request;
using Expenso.BudgetSharing.Proxy.DTO.API.CreateBudgetPermission.Response;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissions;

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
        HttpResponseMessage testResult =
            await _httpClient.PostAsJsonAsync(requestUri: requestPath, value: createBudgetPermissionRequest);

        // Assert
        testResult.StatusCode.Should().Be(expected: HttpStatusCode.Created);

        CreateBudgetPermissionResponse? createBudgetPermissionResponse =
            await testResult.Content.ReadFromJsonAsync<CreateBudgetPermissionResponse>();

        createBudgetPermissionResponse?.BudgetPermissionId.Should().Be(expected: budgetPermissionId);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        const string requestPath = "budget-sharing/budget-permissions";

        // Act
        HttpResponseMessage testResult = await _httpClient.PostAsync(requestUri: requestPath, content: null);

        // Assert
        testResult.StatusCode.Should().Be(expected: HttpStatusCode.Unauthorized);
    }
}