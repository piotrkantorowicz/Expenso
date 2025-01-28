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
    public async Task Should_ReturnExpectedResult_And_Defualts()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: ApiRequestUrl);

        // Assert
        AssertResponseOk(response: response);

        PagedList<GetJobEntriesResponse>? responseContent =
            await response.Content.ReadFromJsonAsync<PagedList<GetJobEntriesResponse>>();

        responseContent?.Items.ShouldNotBeNull();
        responseContent?.CurrentPage.ShouldBe(expected: PaginationDefaults.Page);
        responseContent?.ResultsPerPage.ShouldBe(expected: PaginationDefaults.Limit);
        responseContent?.TotalPages.ShouldBeGreaterThanOrEqualTo(expected: 1);
        responseContent?.TotalResults.ShouldBeGreaterThanOrEqualTo(expected: 1);
        responseContent?.ShouldNotBeNull();
    }

    [Test, TestCase(arg1: "1", arg2: "25", TestName = "Should_UseDefaultPageAndLimit_When_DefaultPaginationProvided"),
     TestCase(arg1: "2", arg2: "5", TestName = "Should_UseProvidedPageAndLimit_When_CustomPaginationProvided")]
    public async Task Should_HandleValidPaginationParameters(string? page, string? limit)
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);

        // Act
        HttpResponseMessage response =
            await _httpClient.GetAsync(requestUri: $"{ApiRequestUrl}?pagination={page},{limit}");

        // Assert
        AssertResponseOk(response: response);

        IPagedList<GetJobEntriesResponse>? responseContent = await response.Content
            .ReadFromJsonAsync<PagedList<GetJobEntriesResponse>>();

        responseContent.ShouldNotBeNull();
        responseContent.CurrentPage.ShouldBeGreaterThanOrEqualTo(expected: PaginationDefaults.Page);
        responseContent.ResultsPerPage.ShouldBeGreaterThan(expected: 0);
        responseContent.ResultsPerPage.ShouldBeLessThanOrEqualTo(expected: PaginationDefaults.MaxLimit);
    }

    [TestCase(arg1: "0", arg2: "10", TestName = "Should_UseDefaultPage_When_PageIsZero"),
     TestCase(arg1: "-1", arg2: "10", TestName = "Should_UseDefaultPage_When_PageIsNegative"),
     TestCase(arg1: "1", arg2: "0", TestName = "Should_UseDefaultPage_And_UseDefaultLimit_When_LimitIsZero"),
     TestCase(arg1: "1", arg2: "1001", TestName = "Should_UseDefaultPage_And_UseMaxLimit_When_LimitExceedsMaximum"),
     TestCase(arg1: "1", arg2: "-10", TestName = "Should_UseDefaultPageAndLimit_When_LimitIsLessThanMinimum"),
     TestCase(arg1: "2147483647", arg2: "2147483647", TestName = "Should_HandleMaximumPageAndLimitValues"),
     TestCase(arg1: "0", arg2: "0", TestName = "Should_HandleNullPageAndLimitValues"),
     TestCase(arg1: null, arg2: null, TestName = "Should_HandleNullPageAndLimitValues"),
     TestCase(arg1: "abc", arg2: "xyz", TestName = "Should_HandleNonIntegerPageAndLimitValues"),
     TestCase(arg1: "", arg2: "", TestName = "Should_HandleEmptyStringPageAndLimitValues")]
    public async Task Should_HandleInvalidPaginationParameters(string? page, string? limit)
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);

        // Act
        HttpResponseMessage response =
            await _httpClient.GetAsync(requestUri: $"{ApiRequestUrl}?pagination={page},{limit}");

        // Assert
        AssertResponseBadRequest(response: response);
    }

    [Test]
    public async Task Should_HandleSingleSorter()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);

        // Act
        HttpResponseMessage response =
            await _httpClient.GetAsync(requestUri: $"{ApiRequestUrl}?sorters=RunAt:Descending");

        // Assert
        AssertResponseOk(response: response);

        IPagedList<GetJobEntriesResponse>? responseContent = await response.Content
            .ReadFromJsonAsync<PagedList<GetJobEntriesResponse>>();

        responseContent.ShouldNotBeNull();
        responseContent.CurrentPage.ShouldBeGreaterThanOrEqualTo(expected: PaginationDefaults.Page);

        responseContent
            .Items.Select(selector: x => x.RunAt)
            .ShouldBeInOrder(expectedSortDirection: SortDirection.Descending);
    }

    [Test]
    public async Task Should_HandleMultipleSorters()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);
        // Act
        HttpResponseMessage response =
            await _httpClient.GetAsync(requestUri: $"{ApiRequestUrl}?sorters=MaxRetries:Ascending,RunAt:Descending");

        // Assert
        AssertResponseOk(response: response);

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
        AssertResponseUnauthroised(response: response);
    }
}