using System.Net;
using System.Net.Http.Json;

using Expenso.Api.Tests.E2E.TestData.IAM;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.CreateBudgetPermission.Request;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.CreateBudgetPermission.Response;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissions;

[TestFixture]
internal sealed class CreateBudgetPermission : BudgetPermissionTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: Claims);
        const string budgetCode = "BDGT/11/12/2024";
        Guid budgetPermissionId = Guid.CreateVersion7();
        Guid budgetId = Guid.CreateVersion7();

        CreateBudgetPermissionRequest createBudgetPermissionRequest = new(BudgetPermissionId: budgetPermissionId,
            BudgetId: budgetId, OwnerId: UserDataInitializer.UserIds[index: 3], BudgetCode: budgetCode);

        // Act
        HttpResponseMessage response =
            await _httpClient.PostAsJsonAsync(requestUri: ApiRequestUrl, value: createBudgetPermissionRequest);

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.Created);

        CreateBudgetPermissionResponse? createBudgetPermissionResponse =
            await response.Content.ReadFromJsonAsync<CreateBudgetPermissionResponse>();

        createBudgetPermissionResponse?.BudgetPermissionId.ShouldBe(expected: budgetPermissionId);
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