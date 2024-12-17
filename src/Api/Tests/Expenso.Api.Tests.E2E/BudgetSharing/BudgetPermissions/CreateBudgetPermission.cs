using System.Net;
using System.Net.Http.Json;

using Expenso.Api.Tests.E2E.TestData.IAM;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.CreateBudgetPermission.DTO.Request;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.CreateBudgetPermission.DTO.Response;

using FluentAssertions;

using NUnit.Framework;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissions;

[TestFixture]
internal sealed class CreateBudgetPermission : BudgetPermissionTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);
        const string requestPath = "budget-sharing/budget-permissions";
        const string budgetCode = "BDGT/11/12/2024";
        Guid budgetPermissionId = Guid.NewGuid();
        Guid budgetId = Guid.NewGuid();

        CreateBudgetPermissionRequest createBudgetPermissionRequest = new(BudgetPermissionId: budgetPermissionId,
            BudgetId: budgetId, OwnerId: UserDataInitializer.UserIds[index: 3], BudgetCode: budgetCode);

        // Act
        HttpResponseMessage response =
            await _httpClient.PostAsJsonAsync(requestUri: requestPath, value: createBudgetPermissionRequest);

        // Assert
        AssertResponseCreated(response: response);

        CreateBudgetPermissionResponse? createBudgetPermissionResponse =
            await response.Content.ReadFromJsonAsync<CreateBudgetPermissionResponse>();

        createBudgetPermissionResponse?.BudgetPermissionId.Should().Be(expected: budgetPermissionId);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        const string requestPath = "budget-sharing/budget-permissions";

        // Act
        HttpResponseMessage response = await _httpClient.PostAsync(requestUri: requestPath, content: null);

        // Assert
        AssertResponseUnauthroised(response: response);
    }
}