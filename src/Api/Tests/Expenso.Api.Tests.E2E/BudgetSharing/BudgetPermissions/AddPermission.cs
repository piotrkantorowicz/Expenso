using System.Net;
using System.Net.Http.Json;

using Expenso.Api.Tests.E2E.TestData.BudgetSharing;
using Expenso.Api.Tests.E2E.TestData.IAM;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.AddPermission.Request;

using NUnit.Framework;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissions;

[TestFixture]
internal sealed class AddPermission : BudgetPermissionTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: Claims);

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
            requestUri:
            $"{ApiRequestUrl}/{BudgetSharingDataInitializer.BudgetPermissionIds[index: 0]}/participants/{UserDataInitializer.UserIds[index: 4]}",
            value: AddPermissionRequestPermissionType.Reviewer);

        // Assert
        AssertResponseNoContent(response: response);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        // Act
        HttpResponseMessage response = await _httpClient.PostAsync(
            requestUri:
            $"{ApiRequestUrl}/{BudgetSharingDataInitializer.BudgetPermissionIds[index: 0]}/participants/{UserDataInitializer.UserIds[index: 3]}",
            content: null);

        // Assert
        AssertResponseUnauthroised(response: response);
    }
}