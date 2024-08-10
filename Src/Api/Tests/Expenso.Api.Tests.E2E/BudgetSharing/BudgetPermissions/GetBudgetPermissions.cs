using Expenso.BudgetSharing.Proxy.DTO.API.GetBudgetPermissions.Response;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissions;

internal sealed class GetBudgetPermissions : BudgetPermissionTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);
        const string requestPath = "budget-sharing/budget-permissions";

        // Act
        HttpResponseMessage testResult = await _httpClient.GetAsync(requestUri: requestPath);

        // Assert
        testResult.StatusCode.Should().Be(expected: HttpStatusCode.OK);

        IEnumerable<GetBudgetPermissionsResponse>? response =
            await testResult.Content.ReadFromJsonAsync<IEnumerable<GetBudgetPermissionsResponse>>();

        response?.Should().NotBeEmpty();
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        const string requestPath = "budget-sharing/budget-permissions";

        // Act
        HttpResponseMessage testResult = await _httpClient.GetAsync(requestUri: requestPath);

        // Assert
        testResult.StatusCode.Should().Be(expected: HttpStatusCode.Unauthorized);
    }
}