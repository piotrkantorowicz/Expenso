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
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);

        // Act
        HttpResponseMessage response =
            await _httpClient.GetAsync(
                requestUri: $"{ApiRequestUrl}/{BudgetPermissionDataInitializer.BudgetPermissionIds[index: 0]}");

        // Assert
        AssertResponseOk(response: response);

        GetBudgetPermissionResponse? responseContent =
            await response.Content.ReadFromJsonAsync<GetBudgetPermissionResponse>();

        responseContent?.Id.ShouldBe(expected: BudgetPermissionDataInitializer.BudgetPermissionIds[index: 0]);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        // Act
        HttpResponseMessage response =
            await _httpClient.GetAsync(
                requestUri: $"{ApiRequestUrl}/{BudgetPermissionDataInitializer.BudgetPermissionIds[index: 0]}");

        // Assert
        AssertResponseUnauthroised(response: response);
    }
}