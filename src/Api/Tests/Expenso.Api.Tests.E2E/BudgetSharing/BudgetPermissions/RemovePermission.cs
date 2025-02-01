using System.Net;

using Expenso.Api.Tests.E2E.TestData.BudgetSharing;
using Expenso.Api.Tests.E2E.TestData.IAM;

using NUnit.Framework;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissions;

[TestFixture]
internal sealed class RemovePermission : BudgetPermissionTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: Claims);

        // Act
        HttpResponseMessage response = await _httpClient.DeleteAsync(
            requestUri:
            $"{ApiRequestUrl}/{BudgetSharingDataInitializer.BudgetPermissionIds[index: 0]}/participants/{UserDataInitializer.UserIds[index: 3]}");

        // Assert
        AssertResponseNoContent(response: response);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        // Act
        HttpResponseMessage response = await _httpClient.DeleteAsync(
            requestUri:
            $"{ApiRequestUrl}/{BudgetSharingDataInitializer.BudgetPermissionIds[index: 0]}/participants/{UserDataInitializer.UserIds[index: 3]}");

        // Assert
        AssertResponseUnauthroised(response: response);
    }
}