using Expenso.Api.Tests.E2E.TestData.BudgetSharing;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissionRequests;

internal sealed class ConfirmAssigningParticipant : BudgetPermissionRequestTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);

        string requestPath =
            $"budget-sharing/budget-permission-requests/{BudgetPermissionDataInitializer.BudgetPermissionRequestIds[index: 1]}/confirm";

        // Act
        HttpResponseMessage testResult = await _httpClient.PatchAsync(requestUri: requestPath, content: null);

        // Assert
        testResult.StatusCode.Should().Be(expected: HttpStatusCode.NoContent);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        string requestPath =
            $"budget-sharing/budget-permission-requests/{BudgetPermissionDataInitializer.BudgetPermissionRequestIds[index: 1]}/confirm";

        // Act
        HttpResponseMessage testResult = await _httpClient.PatchAsync(requestUri: requestPath, content: null);

        // Assert
        testResult.StatusCode.Should().Be(expected: HttpStatusCode.Unauthorized);
    }
}