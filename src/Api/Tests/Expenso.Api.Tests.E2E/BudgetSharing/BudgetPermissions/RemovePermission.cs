using System.Net;

using Expenso.Api.Tests.E2E.TestData.BudgetSharing;
using Expenso.Api.Tests.E2E.TestData.IAM;

using NUnit.Framework;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissions;

[TestFixture]
internal sealed class RemovePermission : BudgetPermissionTestBase
{
    [Test]
    public async Task Should_Return204_When_InputIsValid()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());

        // Act
        HttpResponseMessage response = await _httpClient.DeleteAsync(
            requestUri:
            $"{ApiRequestUrl}/{BudgetSharingDataInitializer.BudgetPermissionIds[index: 0]}/participants/{UserDataInitializer.UserIds[index: 3]}");

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.NoContent);
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
        AssertResponseStatusCode(response: response, statusCode: HttpStatusCode.Unauthorized);
    }
}