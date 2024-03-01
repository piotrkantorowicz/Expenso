namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissionRequests;

internal sealed class GetBudgetPermissionRequests : BudgetPermissionRequestTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(_claims);

        const string requestPath =
            "budget-sharing/budget-permission-requests?status=1&budgetId=527336da-3371-45a9-9b9f-bbd42d01ffc2";

        // Act
        HttpResponseMessage testResult = await _httpClient.GetAsync(requestPath);

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.OK);

        // TODO: Fix this test
        // GetBudgetPermissionRequestResponse? testResultContent =
        //     await testResult.Content.ReadFromJsonAsync<GetBudgetPermissionRequestResponse>();

        // testResultContent?.Id.Should().Be(budgetPermissionRequestId);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        const string requestPath = "budget-sharing/budget-permission-requests";

        // Act
        HttpResponseMessage testResult = await _httpClient.GetAsync(requestPath);

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}