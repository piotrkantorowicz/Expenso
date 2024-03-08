using Expenso.Api.Tests.E2E.TestData.BudgetSharing;
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
        _httpClient.SetFakeBearerToken(_claims);
        const string requestPath = "budget-sharing/budget-permissions";
        Guid budgetPermissionId = Guid.NewGuid();

        CreateBudgetPermissionRequest createBudgetPermissionRequest = new(budgetPermissionId,
            BudgetPermissionDataInitializer.BudgetIds[0], UserDataInitializer.UserIds[3]);

        // Act
        HttpResponseMessage testResult = await _httpClient.PostAsJsonAsync(requestPath, createBudgetPermissionRequest);

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.Created);

        CreateBudgetPermissionResponse? createBudgetPermissionResponse =
            await testResult.Content.ReadFromJsonAsync<CreateBudgetPermissionResponse>();

        createBudgetPermissionResponse?.BudgetPermissionId.Should().Be(budgetPermissionId);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        const string requestPath = "budget-sharing/budget-permissions";

        // Act
        HttpResponseMessage testResult = await _httpClient.PostAsync(requestPath, null);

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}