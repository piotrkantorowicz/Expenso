using System.Net;

using Expenso.Api.Tests.E2E.TestData.TimeManagement;

using NUnit.Framework;

namespace Expenso.Api.Tests.E2E.TimeManagement.JobEntries;

[TestFixture]
internal sealed class CancelJobEntry : JobEntriesTestBase
{
    [Test]
    public async Task Should_CancelJobEntry()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);

        // Act
        HttpResponseMessage response =
            await _httpClient.DeleteAsync(
                requestUri: $"{ApiRequestUrl}/{TimeManagementDataInitializer.JobEntriesIds[index: 1]}");

        // Assert
        AssertResponseNoContent(response: response);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        // Act
        HttpResponseMessage response =
            await _httpClient.DeleteAsync(
                requestUri: $"{ApiRequestUrl}/{TimeManagementDataInitializer.JobEntriesIds[index: 1]}");

        // Assert
        AssertResponseUnauthroised(response: response);
    }
}