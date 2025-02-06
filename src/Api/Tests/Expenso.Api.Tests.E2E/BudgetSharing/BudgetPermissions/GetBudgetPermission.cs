using System.Net;
using System.Net.Http.Json;

using Expenso.Api.Tests.E2E.TestData.BudgetSharing;
using Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermission.DTO.Response;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissions;

[TestFixture]
internal sealed class GetBudgetPermission : BudgetPermissionTestBase
{
    [Test]
    public async Task Should_Return200_When_InputIsValid()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());
        Guid budgetPermissionId = BudgetSharingDataInitializer.BudgetPermissionIds[index: 0];

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: $"{ApiRequestUrl}/{budgetPermissionId}");

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.OK);

        GetBudgetPermissionResponse? responseContent =
            await response.Content.ReadFromJsonAsync<GetBudgetPermissionResponse>();

        responseContent?.Id.ShouldBe(expected: budgetPermissionId);
    }

    [Test]
    public async Task Should_Return404_When_ResourceNotFound()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());
        Guid budgetPermissionId = Guid.CreateVersion7();

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: $"{ApiRequestUrl}/{budgetPermissionId}");

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        Guid budgetPermissionId = BudgetSharingDataInitializer.BudgetPermissionIds[index: 0];

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: $"{ApiRequestUrl}/{budgetPermissionId}");

        // Assert
        AssertResponseStatusCode(response: response, statusCode: HttpStatusCode.Unauthorized);
    }
}