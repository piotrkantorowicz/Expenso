using FluentAssertions;

using Moq;

namespace Expenso.DocumentManagement.Tests.UnitTests.Application.Shared.DirectoryInfoService;

internal sealed class GetImportsDirectory : DirectoryInfoServiceTestBase
{
    [Test]
    public void Should_ReturnCorrectImportDirectoryPath()
    {
        // Arrange
        const string date = "20220101";
        string userId = Guid.NewGuid().ToString();
        string expectedPath = $"RootPath/{userId}/Imports/20220101/group1/group2";

        string[] groups =
        [
            "group1",
            "group2"
        ];

        _fileSystemMock?.Setup(x => x.Path.Combine(It.IsAny<string[]>())).Returns<string[]>(x => string.Join("/", x));

        // Act
        string result = TestCandidate.GetImportsDirectory(userId, groups, date);

        // Assert
        result.Should().Be(expectedPath);
    }
}