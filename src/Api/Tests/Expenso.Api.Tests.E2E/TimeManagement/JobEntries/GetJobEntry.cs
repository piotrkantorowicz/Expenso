using Expenso.Api.Tests.E2E.TestData.TimeManagement;
using Expenso.TimeManagement.Shared.DTO.GetJobEntry.Response;

namespace Expenso.Api.Tests.E2E.TimeManagement.JobEntries;

internal sealed class GetJobEntry : JobEntriesTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        Guid jobEntryId = TimeManagementDataInitializer.JobEntriesIds[index: 2];
        _httpClient.SetFakeBearerToken(token: _claims);
        string requestPath = $"time-management/job-entries/{jobEntryId}";

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: requestPath);

        // Assert
        AssertResponseOk(response: response);
        GetJobEntryResponse? responseContent = await response.Content.ReadFromJsonAsync<GetJobEntryResponse>();
        responseContent?.Id.Should().Be(expected: jobEntryId);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        string requestPath = $"time-management/job-entries/{TimeManagementDataInitializer.JobEntriesIds[index: 2]}";

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: requestPath);

        // Assert
        AssertResponseUnauthroised(response: response);
    }
}