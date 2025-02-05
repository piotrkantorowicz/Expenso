using System.Net;
using System.Net.Http.Json;

using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Response;
using Expenso.Shared.System.Types.Paging;
using Expenso.Shared.System.Types.Paging.Constants;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissions;

[TestFixture]
internal sealed class GetBudgetPermissions : BudgetPermissionTestBase
{
    [Test]
    public async Task Should_Return200_When_InputIsValid_And_DefaultPaginationProvided()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: ApiRequestUrl);

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.OK);

        IPagedList<GetBudgetPermissionsResponse>? responseContent =
            await response.Content.ReadFromJsonAsync<PagedList<GetBudgetPermissionsResponse>>();

        responseContent?.ShouldNotBeNull();
    }

    [Test,
     TestCase(arg1: 1, arg2: 25,
         TestName = "Should_UseDefaultPageAndLimit_When_DefaultPaginationProvided_And_Return200"),
     TestCase(arg1: 2, arg2: 5,
         TestName = "Should_UseProvidedPageAndLimit_When_CustomPaginationProvided_And_Return200")]
    public async Task Should_HandleValidPaginationParameters(int? page, int? limit)
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());

        // Act
        HttpResponseMessage response =
            await _httpClient.GetAsync(requestUri: $"{ApiRequestUrl}?pagination={page},{limit}");

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.OK);

        IPagedList<GetBudgetPermissionsResponse>? responseContent = await response.Content
            .ReadFromJsonAsync<PagedList<GetBudgetPermissionsResponse>>();

        responseContent.ShouldNotBeNull();
        responseContent.CurrentPage.ShouldBeGreaterThanOrEqualTo(expected: PaginationDefaults.Page);
        responseContent.ResultsPerPage.ShouldBeGreaterThan(expected: 0);
        responseContent.ResultsPerPage.ShouldBeLessThanOrEqualTo(expected: PaginationDefaults.MaxLimit);
    }

    [Test, TestCase(arg1: 0, arg2: 10, TestName = "Should_UseDefaultPage_When_PageIsZero_And_Return400"),
     TestCase(arg1: -1, arg2: 10, TestName = "Should_UseDefaultPage_When_PageIsNegative_And_Return400"),
     TestCase(arg1: 1, arg2: 0, TestName = "Should_UseDefaultPage_And_UseDefaultLimit_When_LimitIsZero_And_Return400"),
     TestCase(arg1: 1, arg2: 1001,
         TestName = "Should_UseDefaultPage_And_UseMaxLimit_When_LimitExceedsMaximum_And_Return400"),
     TestCase(arg1: 1, arg2: -10, TestName = "Should_UseDefaultPageAndLimit_When_LimitIsLessThanMinimum_And_Return400"),
     TestCase(arg1: int.MaxValue, arg2: int.MaxValue,
         TestName = "Should_HandleMaximumPageAndLimitValues_And_Return400"),
     TestCase(arg1: null, arg2: null, TestName = "Should_HandleNullPageAndLimitValues_And_Return400")]
    public async Task Should_HandleInvalidPaginationParameters(int? page, int? limit)
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());

        // Act
        HttpResponseMessage response =
            await _httpClient.GetAsync(requestUri: $"{ApiRequestUrl}?pagination={page},{limit}");

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task Should_HandleSingleSorter_And_Return200_When_InputIsValid()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());

        // Act
        HttpResponseMessage response =
            await _httpClient.GetAsync(requestUri: $"{ApiRequestUrl}?sorters=BudgetId:Descending");

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.OK);

        IPagedList<GetBudgetPermissionsResponse>? responseContent = await response.Content
            .ReadFromJsonAsync<PagedList<GetBudgetPermissionsResponse>>();

        responseContent.ShouldNotBeNull();
        responseContent.CurrentPage.ShouldBeGreaterThanOrEqualTo(expected: PaginationDefaults.Page);

        responseContent
            .Items.Select(selector: x => x.BudgetId)
            .ShouldBeInOrder(expectedSortDirection: SortDirection.Descending);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: ApiRequestUrl);

        // Assert
        AssertResponseStatusCode(response: response, statusCode: HttpStatusCode.Unauthorized);
    }
}