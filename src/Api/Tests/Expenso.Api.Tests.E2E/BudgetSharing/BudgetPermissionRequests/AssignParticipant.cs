using Expenso.Api.Tests.E2E.IAM;
using Expenso.Api.Tests.E2E.TestData.BudgetSharing;
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
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUri: requestPath,
            value: new AssignParticipantRequest(BudgetId: BudgetPermissionDataInitializer.BudgetIds[index: 1],
                Email: FakeIamProxy.ExistingEmails[2], PermissionType: AssignParticipantRequest_PermissionType.Reviewer,
                ExpirationDays: 3));

        // Assert
        AssertResponseCreated(response: response);

        AssignParticipantResponse? responseContent =
            await response.Content.ReadFromJsonAsync<AssignParticipantResponse>();

        responseContent?.BudgetPermissionRequestId.Should().NotBeEmpty();
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        const string requestPath = "budget-sharing/budget-permission-requests";

        // Act
        HttpResponseMessage response = await _httpClient.PostAsync(requestUri: requestPath, content: null);

        // Assert
        AssertResponseUnauthroised(response: response);
    }
}