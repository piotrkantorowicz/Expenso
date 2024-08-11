using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Response;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissionRequests;

internal sealed class GetBudgetPermissionRequests : BudgetPermissionRequestTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);

        const string requestPath =
            "budget-sharing/budget-permission-requests?status=1&budgetId=527336da-3371-45a9-9b9f-bbd42d01ffc2";

        // Act
        HttpResponseMessage testResult = await _httpClient.GetAsync(requestUri: requestPath);

        // Assert
        testResult.StatusCode.Should().Be(expected: HttpStatusCode.OK);

        IEnumerable<GetBudgetPermissionRequestsResponse>? testResultContent =
            await testResult.Content.ReadFromJsonAsync<IEnumerable<GetBudgetPermissionRequestsResponse>>();

        testResultContent?.Should().NotBeNull();
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        const string requestPath = "budget-sharing/budget-permission-requests";

        // Act
        HttpResponseMessage testResult = await _httpClient.GetAsync(requestUri: requestPath);

        // Assert
        testResult.StatusCode.Should().Be(expected: HttpStatusCode.Unauthorized);
    }
}