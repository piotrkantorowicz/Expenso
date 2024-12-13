using Expenso.TimeManagement.Core.Application.JobEntries.Read.GetJobEntries.DTO.Response;

namespace Expenso.Api.Tests.E2E.TimeManagement.JobEntries;

internal sealed class GetJobEntries : JobEntriesTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);
        const string requestPath = "time-management/job-entries";

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: requestPath);

        // Assert
        AssertResponseOk(response: response);

        IReadOnlyCollection<GetJobEntriesResponse>? responseContent =
            await response.Content.ReadFromJsonAsync<IReadOnlyCollection<GetJobEntriesResponse>>();

        responseContent?.Should().NotBeNull();
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