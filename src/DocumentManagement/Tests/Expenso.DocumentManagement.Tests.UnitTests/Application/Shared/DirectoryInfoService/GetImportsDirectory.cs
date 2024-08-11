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

        _fileSystemMock
            ?.Setup(expression: x => x.Path.Combine(It.IsAny<string[]>()))
            .Returns<string[]>(valueFunction: x => string.Join(separator: "/", value: x));

        // Act
        string result = TestCandidate.GetImportsDirectory(userId: userId, groups: groups, date: date);

        // Assert
        result.Should().Be(expected: expectedPath);
    }
}