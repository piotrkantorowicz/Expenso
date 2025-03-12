using System.Net;
using System.Net.Http.Json;

using Expenso.Shared.System.Types.Paging;
using Expenso.Shared.System.Types.Paging.Constants;
using Expenso.TimeManagement.Core.Application.JobEntries.Read.GetJobEntries.DTO.Response;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Api.Tests.E2E.TimeManagement.JobEntries;

internal sealed class GetJobEntries : JobEntriesTestBase
{
    [Test]
    public async Task Should_Return200_When_InputIsValid_And_DefualtPaginationProvided()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: ApiRequestUrl);

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.OK);

        PagedList<GetJobEntriesResponse>? responseContent =
            await response.Content.ReadFromJsonAsync<PagedList<GetJobEntriesResponse>>();

        responseContent?.Items.ShouldNotBeNull();
        responseContent?.CurrentPage.ShouldBe(expected: PaginationDefaults.Page);
        responseContent?.ResultsPerPage.ShouldBe(expected: PaginationDefaults.Limit);
        responseContent?.TotalPages.ShouldBeGreaterThanOrEqualTo(expected: 1);
        responseContent?.TotalResults.ShouldBeGreaterThanOrEqualTo(expected: 1);
        responseContent?.ShouldNotBeNull();
    }

    [Test,
     TestCase(arg1: "1", arg2: "25",
         TestName = "Should_UseDefaultPageAndLimit_When_DefaultPaginationProvided_And_Return200"),
     TestCase(arg1: "2", arg2: "5",
         TestName = "Should_UseProvidedPageAndLimit_When_CustomPaginationProvided_And_Return200")]
    public async Task Should_HandleValidPaginationParameters(string? page, string? limit)
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());

        // Act
        HttpResponseMessage response =
            await _httpClient.GetAsync(requestUri: $"{ApiRequestUrl}?pagination={page},{limit}");

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.OK);

        IPagedList<GetJobEntriesResponse>? responseContent = await response.Content
            .ReadFromJsonAsync<PagedList<GetJobEntriesResponse>>();

        responseContent.ShouldNotBeNull();
        responseContent.CurrentPage.ShouldBeGreaterThanOrEqualTo(expected: PaginationDefaults.Page);
        responseContent.ResultsPerPage.ShouldBeGreaterThan(expected: 0);
        responseContent.ResultsPerPage.ShouldBeLessThanOrEqualTo(expected: PaginationDefaults.MaxLimit);
    }

    [TestCase(arg1: "0", arg2: "10", TestName = "Should_UseDefaultPage_When_PageIsZero_And_Return400"),
     TestCase(arg1: "-1", arg2: "10", TestName = "Should_UseDefaultPage_When_PageIsNegative_And_Return400"),
     TestCase(arg1: "1", arg2: "0",
         TestName = "Should_UseDefaultPage_And_UseDefaultLimit_When_LimitIsZero_And_Return400"),
     TestCase(arg1: "1", arg2: "1001",
         TestName = "Should_UseDefaultPage_And_UseMaxLimit_When_LimitExceedsMaximum_And_Return400"),
     TestCase(arg1: "1", arg2: "-10",
         TestName = "Should_UseDefaultPageAndLimit_When_LimitIsLessThanMinimum_And_Return400"),
     TestCase(arg1: "2147483647", arg2: "2147483647",
         TestName = "Should_HandleMaximumPageAndLimitValues_And_Return400"),
     TestCase(arg1: "0", arg2: "0", TestName = "Should_HandleNullPageAndLimitValues_And_Return400"),
     TestCase(arg1: null, arg2: null, TestName = "Should_HandleNullPageAndLimitValues_And_Return400"),
     TestCase(arg1: "abc", arg2: "xyz", TestName = "Should_HandleNonIntegerPageAndLimitValues_And_Return400"),
     TestCase(arg1: "", arg2: "", TestName = "Should_HandleEmptyStringPageAndLimitValues_And_Return400")]
    public async Task Should_HandleInvalidPaginationParameters(string? page, string? limit)
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
            await _httpClient.GetAsync(requestUri: $"{ApiRequestUrl}?sorters=RunAt:Descending");

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.OK);

        IPagedList<GetJobEntriesResponse>? responseContent = await response.Content
            .ReadFromJsonAsync<PagedList<GetJobEntriesResponse>>();

        responseContent.ShouldNotBeNull();
        responseContent.CurrentPage.ShouldBeGreaterThanOrEqualTo(expected: PaginationDefaults.Page);

        responseContent
            .Items.Select(selector: x => x.RunAt)
            .ShouldBeInOrder(expectedSortDirection: SortDirection.Descending);
    }

    [Test]
    public async Task Should_HandleMultipleSorters_And_Return200_When_InputIsValid()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());
        // Act
        HttpResponseMessage response =
            await _httpClient.GetAsync(requestUri: $"{ApiRequestUrl}?sorters=MaxRetries:Ascending,RunAt:Descending");

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.OK);

        IPagedList<GetJobEntriesResponse>? responseContent = await response.Content
            .ReadFromJsonAsync<PagedList<GetJobEntriesResponse>>();

        responseContent.ShouldNotBeNull();
        responseContent.CurrentPage.ShouldBeGreaterThanOrEqualTo(expected: PaginationDefaults.Page);

        responseContent
            .Items.Select(selector: x => x.MaxRetries)
            .ShouldBeInOrder(expectedSortDirection: SortDirection.Ascending);

        responseContent
            .Items.Select(selector: x => x.RunAt)
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