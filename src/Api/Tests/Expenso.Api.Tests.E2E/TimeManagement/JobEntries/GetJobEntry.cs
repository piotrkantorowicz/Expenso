using System.Net;
using System.Net.Http.Json;

using Expenso.Api.Tests.E2E.TestData.TimeManagement;
using Expenso.TimeManagement.Shared.DTO.GetJobEntry.Response;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Api.Tests.E2E.TimeManagement.JobEntries;

internal sealed class GetJobEntry : JobEntriesTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        Guid jobEntryId = TimeManagementDataInitializer.JobEntriesIds[index: 2];
        _httpClient.SetFakeBearerToken(token: Claims);

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: $"{ApiRequestUrl}/{jobEntryId}");

        // Assert
        AssertResponseOk(response: response);
        GetJobEntryResponse? responseContent = await response.Content.ReadFromJsonAsync<GetJobEntryResponse>();
        responseContent?.Id.ShouldBe(expected: jobEntryId);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        // Act
        HttpResponseMessage response =
            await _httpClient.GetAsync(
                requestUri: $"{ApiRequestUrl}/{TimeManagementDataInitializer.JobEntriesIds[index: 2]}");

        // Assert
        AssertResponseUnauthroised(response: response);
    }
}