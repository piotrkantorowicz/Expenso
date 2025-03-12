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
    public async Task Should_Return201_When_InputIsValid()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());
        Guid budgetPermissioRequestId = Guid.CreateVersion7();

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUri: ApiRequestUrl,
            value: new AssignParticipantRequest(BudgetPermissionRequestId: budgetPermissioRequestId,
                BudgetId: BudgetSharingDataInitializer.BudgetIds[index: 1], Email: FakeIamProxy.ExistingEmails[2],
                PermissionType: AssignParticipantRequestPermissionType.Reviewer));

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.Created);

        AssignParticipantResponse? responseContent =
            await response.Content.ReadFromJsonAsync<AssignParticipantResponse>();

        responseContent?.BudgetPermissionRequestId.ShouldBe(expected: budgetPermissioRequestId);
    }

    [Test]
    public async Task Should_Return422_When_BudgetPermissionIdIsEmpty()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());
        Guid budgetPermissioRequestId = Guid.Empty;

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUri: ApiRequestUrl,
            value: new AssignParticipantRequest(BudgetPermissionRequestId: budgetPermissioRequestId,
                BudgetId: BudgetSharingDataInitializer.BudgetIds[index: 1], Email: FakeIamProxy.ExistingEmails[2],
                PermissionType: AssignParticipantRequestPermissionType.Reviewer));

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.UnprocessableEntity);
    }

    [Test]
    public async Task Should_Return422_When_BudgetIdIsEmpty()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());
        Guid budgetPermissioRequestId = Guid.CreateVersion7();
        Guid budgetId = Guid.Empty;

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUri: ApiRequestUrl,
            value: new AssignParticipantRequest(BudgetPermissionRequestId: budgetPermissioRequestId, BudgetId: budgetId,
                Email: FakeIamProxy.ExistingEmails[2],
                PermissionType: AssignParticipantRequestPermissionType.Reviewer));

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.UnprocessableEntity);
    }

    [Test]
    public async Task Should_Return422_When_BudgetNotFound()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());
        Guid budgetPermissioRequestId = Guid.CreateVersion7();
        Guid budgetId = Guid.CreateVersion7();

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUri: ApiRequestUrl,
            value: new AssignParticipantRequest(BudgetPermissionRequestId: budgetPermissioRequestId, BudgetId: budgetId,
                Email: FakeIamProxy.ExistingEmails[2],
                PermissionType: AssignParticipantRequestPermissionType.Reviewer));

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.UnprocessableEntity);
    }

    [Test]
    public async Task Should_Return422_When_EmailIsInvalid()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());
        Guid budgetPermissioRequestId = Guid.CreateVersion7();

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUri: ApiRequestUrl,
            value: new AssignParticipantRequest(BudgetPermissionRequestId: budgetPermissioRequestId,
                BudgetId: BudgetSharingDataInitializer.BudgetIds[index: 1], Email: "invalid-email",
                PermissionType: AssignParticipantRequestPermissionType.Reviewer));

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.UnprocessableEntity);
    }

    [Test]
    public async Task Should_Return422_When_UserWithProvidedEmailNotExists()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());
        Guid budgetPermissioRequestId = Guid.CreateVersion7();

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUri: ApiRequestUrl,
            value: new AssignParticipantRequest(BudgetPermissionRequestId: budgetPermissioRequestId,
                BudgetId: BudgetSharingDataInitializer.BudgetIds[index: 1], Email: "john@email.com",
                PermissionType: AssignParticipantRequestPermissionType.Reviewer));

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.UnprocessableEntity);
    }

    [Test]
    public async Task Should_Return422_When_UserWithProvidedEmailHasBeenAlreadyAssignedToThisBudget()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());
        Guid budgetPermissioRequestId = Guid.CreateVersion7();

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUri: ApiRequestUrl,
            value: new AssignParticipantRequest(BudgetPermissionRequestId: budgetPermissioRequestId,
                BudgetId: BudgetSharingDataInitializer.BudgetIds[index: 1], Email: FakeIamProxy.ExistingEmails[0],
                PermissionType: AssignParticipantRequestPermissionType.Reviewer));

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.UnprocessableEntity);
    }

    [Test]
    public async Task Should_Return422_When_UserHasAlreadyOpenedRequestForThisBudget()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());
        Guid budgetPermissioRequestId = Guid.CreateVersion7();

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUri: ApiRequestUrl,
            value: new AssignParticipantRequest(BudgetPermissionRequestId: budgetPermissioRequestId,
                BudgetId: BudgetSharingDataInitializer.BudgetIds[index: 1], Email: FakeIamProxy.ExistingEmails[1],
                PermissionType: AssignParticipantRequestPermissionType.Reviewer));

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.UnprocessableEntity);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        // Act
        HttpResponseMessage response = await _httpClient.PostAsync(requestUri: ApiRequestUrl, content: null);

        // Assert
        AssertResponseStatusCode(response: response, statusCode: HttpStatusCode.Unauthorized);
    }
}