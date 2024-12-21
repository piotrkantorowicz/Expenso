using System.Net;
using System.Net.Http.Json;

using Expenso.Shared.System.Types.Pagination;
using Expenso.Shared.System.Types.Pagination.Constants;
using Expenso.TimeManagement.Core.Application.JobEntries.Read.GetJobEntries.DTO.Response;

using FluentAssertions;

using NUnit.Framework;

namespace Expenso.Api.Tests.E2E.TimeManagement.JobEntries;

internal sealed class GetJobEntries : JobEntriesTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult_And_DefualtPagination()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);
        const string requestPath = "time-management/job-entries";

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: requestPath);

        // Assert
        AssertResponseOk(response: response);

        PagedList<GetJobEntriesResponse>? responseContent =
            await response.Content.ReadFromJsonAsync<PagedList<GetJobEntriesResponse>>();

        responseContent?.Items.Should().NotBeNull();
        responseContent?.CurrentPage.Should().Be(expected: PaginationDefaults.Page);
        responseContent?.ResultsPerPage.Should().Be(expected: PaginationDefaults.Limit);
        responseContent?.TotalPages.Should().BeGreaterThanOrEqualTo(expected: 1);
        responseContent?.TotalResults.Should().BeGreaterThanOrEqualTo(expected: 1);
        responseContent?.Should().NotBeNull();
    }

    [TestCase(arg1: "0", arg2: "10", TestName = "Should_UseDefaultPage_When_PageIsZero"),
     TestCase(arg1: "-1", arg2: "10", TestName = "Should_UseDefaultPage_When_PageIsNegative"),
     TestCase(arg1: "1", arg2: "0", TestName = "Should_UseDefaultPage_And_UseDefaultLimit_When_LimitIsZero"),
     TestCase(arg1: "1", arg2: "1001", TestName = "Should_UseDefaultPage_And_UseMaxLimit_When_LimitExceedsMaximum"),
     TestCase(arg1: "1", arg2: "25", TestName = "Should_UseDefaultPageAndLimit_When_DefaultPaginationProvided"),
     TestCase(arg1: "2", arg2: "5", TestName = "Should_UseProvidedPageAndLimit_When_CustomPaginationProvided"),
     TestCase(arg1: "1", arg2: "-10", TestName = "Should_UseDefaultPageAndLimit_When_LimitIsLessThanMinimum"),
     TestCase(arg1: "2147483647", arg2: "2147483647", TestName = "Should_HandleMaximumPageAndLimitValues"),
     TestCase(arg1: "0", arg2: "0", TestName = "Should_HandleNullPageAndLimitValues"),
     TestCase(arg1: null, arg2: null, TestName = "Should_HandleNullPageAndLimitValues"),
     TestCase(arg1: "abc", arg2: "xyz", TestName = "Should_HandleNonIntegerPageAndLimitValues"),
     TestCase(arg1: "", arg2: "", TestName = "Should_HandleEmptyStringPageAndLimitValues")]
    public async Task Should_HandlePaginationParameters(string? page, string? limit)
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);
        string requestPath = $"time-management/job-entries?pagination={page},{limit}";

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: requestPath);

        // Assert
        AssertResponseOk(response: response);

        IPagedList<GetJobEntriesResponse>? responseContent = await response.Content
            .ReadFromJsonAsync<PagedList<GetJobEntriesResponse>>();

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
        const string requestPath = "time-management/job-entries";

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: requestPath);

        // Assert
        AssertResponseUnauthroised(response: response);
    }
}