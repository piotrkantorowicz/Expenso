using System.Net;
using System.Net.Http.Json;

using Expenso.Api.Configuration.Execution.Middlewares;
using Expenso.Api.Tests.E2E.TestData.BudgetSharing;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequest.DTO.Response;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissionRequests;

[TestFixture]
internal sealed class GetBudgetPermissionRequest : BudgetPermissionRequestTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);
        Guid budgetPermissionRequestId = BudgetSharingDataInitializer.BudgetPermissionRequestIds[index: 2];

        // Act
        HttpResponseMessage response =
            await _httpClient.GetAsync(requestUri: $"{ApiRequestUrl}/{budgetPermissionRequestId}");

        // Assert
        AssertResponseOk(response: response);

        GetBudgetPermissionRequestResponse? responseContent =
            await response.Content.ReadFromJsonAsync<GetBudgetPermissionRequestResponse>();

        response.Headers.Contains(name: CorrelationIdMiddleware.CorrelationHeaderKey).ShouldBeTrue();
        responseContent?.Id.ShouldBe(expected: budgetPermissionRequestId);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(
            requestUri: $"{ApiRequestUrl}/{BudgetSharingDataInitializer.BudgetPermissionRequestIds[index: 2]}");

        // Assert
        AssertResponseUnauthroised(response: response);
    }
}