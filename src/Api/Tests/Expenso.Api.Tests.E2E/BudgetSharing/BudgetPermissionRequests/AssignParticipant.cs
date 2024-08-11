using Expenso.Api.Tests.E2E.IAM;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant.DTO.Request;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant.DTO.Response;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissionRequests;

internal sealed class AssignParticipant : BudgetPermissionRequestTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);
        const string requestPath = "budget-sharing/budget-permission-requests";

        // Act
        HttpResponseMessage testResult = await _httpClient.PostAsJsonAsync(requestUri: requestPath,
            value: new AssignParticipantRequest(BudgetId: Guid.NewGuid(), Email: FakeIamProxy.ExistingEmails[1],
                PermissionType: AssignParticipantRequest_PermissionType.Reviewer, ExpirationDays: 3));

        // Assert
        testResult.StatusCode.Should().Be(expected: HttpStatusCode.Created);

        AssignParticipantResponse? testResultContent =
            await testResult.Content.ReadFromJsonAsync<AssignParticipantResponse>();

        testResultContent?.Should().NotBeNull();
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        const string requestPath = "budget-sharing/budget-permission-requests";

        // Act
        HttpResponseMessage testResult = await _httpClient.PostAsync(requestUri: requestPath, content: null);

        // Assert
        testResult.StatusCode.Should().Be(expected: HttpStatusCode.Unauthorized);
    }
}