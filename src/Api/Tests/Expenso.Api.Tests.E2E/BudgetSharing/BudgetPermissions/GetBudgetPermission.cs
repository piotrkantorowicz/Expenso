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

        // Act
        HttpResponseMessage response =
            await _httpClient.GetAsync(
                requestUri: $"{ApiRequestUrl}/{BudgetSharingDataInitializer.BudgetPermissionIds[index: 0]}");

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.OK);

        GetBudgetPermissionResponse? responseContent =
            await response.Content.ReadFromJsonAsync<GetBudgetPermissionResponse>();

        responseContent?.Id.ShouldBe(expected: BudgetSharingDataInitializer.BudgetPermissionIds[index: 0]);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        // Act
        HttpResponseMessage response =
            await _httpClient.GetAsync(
                requestUri: $"{ApiRequestUrl}/{BudgetSharingDataInitializer.BudgetPermissionIds[index: 0]}");

        // Assert
        AssertResponseStatusCode(response: response, statusCode: HttpStatusCode.Unauthorized);
    }
}