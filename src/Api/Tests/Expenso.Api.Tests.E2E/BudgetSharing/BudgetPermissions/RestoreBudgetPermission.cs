using System.Net;

using Expenso.Api.Tests.E2E.TestData.BudgetSharing;

using NUnit.Framework;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissions;

[TestFixture]
internal sealed class RestoreBudgetPermission : BudgetPermissionTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);

        // Act
        HttpResponseMessage response = await _httpClient.PatchAsync(
            requestUri: $"{ApiRequestUrl}/{BudgetPermissionDataInitializer.BudgetPermissionIds[index: 2]}",
            content: null);

        // Assert
        AssertResponseNoContent(response: response);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        // Act
        HttpResponseMessage response = await _httpClient.PatchAsync(
            requestUri: $"{ApiRequestUrl}/{BudgetPermissionDataInitializer.BudgetPermissionIds[index: 2]}",
            content: null);

        // Assert
        AssertResponseUnauthroised(response: response);
    }
}