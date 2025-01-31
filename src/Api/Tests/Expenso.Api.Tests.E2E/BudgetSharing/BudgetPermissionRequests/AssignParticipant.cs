using System.Net;
using System.Net.Http.Json;

using Expenso.Api.Tests.E2E.IAM;
using Expenso.Api.Tests.E2E.TestData.BudgetSharing;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissionRequests.AssignParticipant.Request;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissionRequests.AssignParticipant.Response;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissionRequests;

[TestFixture]
internal sealed class AssignParticipant : BudgetPermissionRequestTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);
        Guid budgetPermissioRequestId = Guid.CreateVersion7();

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUri: ApiRequestUrl,
            value: new AssignParticipantRequest(BudgetPermissionRequestId: budgetPermissioRequestId,
                BudgetId: BudgetSharingDataInitializer.BudgetIds[index: 1], Email: FakeIamProxy.ExistingEmails[2],
                PermissionType: AssignParticipantRequestPermissionType.Reviewer));

        // Assert
        AssertResponseCreated(response: response);

        AssignParticipantResponse? responseContent =
            await response.Content.ReadFromJsonAsync<AssignParticipantResponse>();

        responseContent?.BudgetPermissionRequestId.ShouldBe(expected: budgetPermissioRequestId);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        // Act
        HttpResponseMessage response = await _httpClient.PostAsync(requestUri: ApiRequestUrl, content: null);

        // Assert
        AssertResponseUnauthroised(response: response);
    }
}