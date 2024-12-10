using Expenso.Api.Tests.E2E.TestData.TimeManagement;

namespace Expenso.Api.Tests.E2E.TimeManagement.JobEntries;

[TestFixture]
internal sealed class CancelJobEntry : JobEntriesTestBase
{
    [Test]
    public async Task Should_CancelJobEntry()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);
        string requestPath = $"time-management/job-entries/{TimeManagementDataInitializer.JobEntriesIds[index: 1]}";

        // Act
        HttpResponseMessage response = await _httpClient.DeleteAsync(requestUri: requestPath);

        // Assert
        AssertResponseNoContent(response: response);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        string requestPath = $"time-management/job-entries/{TimeManagementDataInitializer.JobEntriesIds[index: 1]}";

        // Act
        HttpResponseMessage response = await _httpClient.DeleteAsync(requestUri: requestPath);

        // Assert
        AssertResponseUnauthroised(response: response);
    }
}