using Expenso.Api.Tests.E2E.TestData.BudgetSharing;
using Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermission.DTO.Response;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissions;

internal sealed class GetBudgetPermission : BudgetPermissionTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(_claims);

        string requestPath =
            $"budget-sharing/budget-permissions/{BudgetPermissionDataInitializer.BudgetPermissionIds[0]}";

        // Act
        HttpResponseMessage testResult = await _httpClient.GetAsync(requestPath);

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.OK);

        GetBudgetPermissionResponse? response =
            await testResult.Content.ReadFromJsonAsync<GetBudgetPermissionResponse>();

        response?.Id.Should().Be(BudgetPermissionDataInitializer.BudgetPermissionIds[0]);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        string requestPath =
            $"budget-sharing/budget-permissions/{BudgetPermissionDataInitializer.BudgetPermissionIds[0]}";

        // Act
        HttpResponseMessage testResult = await _httpClient.GetAsync(requestPath);

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}