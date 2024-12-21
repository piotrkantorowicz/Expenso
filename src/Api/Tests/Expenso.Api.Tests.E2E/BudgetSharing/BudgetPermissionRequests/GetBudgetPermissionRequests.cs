using System.Net;
using System.Net.Http.Json;

using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Response;
using Expenso.Shared.System.Types.Pagination;
using Expenso.Shared.System.Types.Pagination.Constants;

using FluentAssertions;

using NUnit.Framework;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissionRequests;

[TestFixture]
internal sealed class GetBudgetPermissionRequests : BudgetPermissionRequestTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);

        const string requestPath =
            "budget-sharing/budget-permission-requests?budgetId=527336da-3371-45a9-9b9f-bbd42d01ffc2";

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: requestPath);

        // Assert
        AssertResponseOk(response: response);

        IPagedList<GetBudgetPermissionRequestsResponse>? responseContent =
            await response.Content.ReadFromJsonAsync<PagedList<GetBudgetPermissionRequestsResponse>?>();

        responseContent?.Should().NotBeNull();
    }

    [TestCase(arg1: 0, arg2: 10, TestName = "Should_UseDefaultPage_When_PageIsZero"),
     TestCase(arg1: -1, arg2: 10, TestName = "Should_UseDefaultPage_When_PageIsNegative"),
     TestCase(arg1: 1, arg2: 0, TestName = "Should_UseDefaultPage_And_UseDefaultLimit_When_LimitIsZero"),
     TestCase(arg1: 1, arg2: 1001, TestName = "Should_UseDefaultPage_And_UseMaxLimit_When_LimitExceedsMaximum"),
     TestCase(arg1: 1, arg2: 25, TestName = "Should_UseDefaultPageAndLimit_When_DefaultPaginationProvided"),
     TestCase(arg1: 2, arg2: 5, TestName = "Should_UseProvidedPageAndLimit_When_CustomPaginationProvided"),
     TestCase(arg1: 1, arg2: -10, TestName = "Should_UseDefaultPageAndLimit_When_LimitIsLessThanMinimum"),
     TestCase(arg1: int.MaxValue, arg2: int.MaxValue, TestName = "Should_HandleMaximumPageAndLimitValues"),
     TestCase(arg1: null, arg2: null, TestName = "Should_HandleNullPageAndLimitValues")]
    public async Task Should_HandlePaginationParameters(int? page, int? limit)
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);
        string requestPath = $"budget-sharing/budget-permission-requests?pagination={page},{limit}";

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: requestPath);

        // Assert
        AssertResponseOk(response: response);

        IPagedList<GetBudgetPermissionRequestsResponse>? responseContent = await response.Content
            .ReadFromJsonAsync<PagedList<GetBudgetPermissionRequestsResponse>>();

        responseContent.Should().NotBeNull();
        responseContent?.CurrentPage.Should().BeGreaterThanOrEqualTo(expected: PaginationDefaults.Page);

        responseContent
            ?.ResultsPerPage.Should()
            .BeGreaterThan(expected: 0)
            .And.BeLessOrEqualTo(expected: PaginationDefaults.MaxLimit);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        const string requestPath = "budget-sharing/budget-permission-requests";

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: requestPath);

        // Assert
        AssertResponseUnauthroised(response: response);
    }
}