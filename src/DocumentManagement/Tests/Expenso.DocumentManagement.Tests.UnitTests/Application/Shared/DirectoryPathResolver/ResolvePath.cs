using FluentAssertions;

using Moq;

namespace Expenso.DocumentManagement.Tests.UnitTests.Application.Shared.DirectoryPathResolver;

[TestFixture]
internal sealed class ResolvePath : DirectoryPathResolverTestBase
{
    [Test]
    public void Should_ReturnCorrectPathForImports()
    {
        // Arrange
        const int fileType = 1;
        string userId = Guid.NewGuid().ToString();

        string expectedPath =
            $"RootPath/{userId}/{Core.Application.Shared.Services.Acl.Disk.DirectoryInfoService.Imports}/20220101/group1/group2";

        string[] groups =
        [
            "group1",
            "group2"
        ];

        _directoryInfoServiceMock
            .Setup(expression: x => x.GetImportsDirectory(userId, groups, It.IsAny<string>()))
            .Returns(value: expectedPath);

        // Act
        string result = TestCandidate.ResolvePath(fileType: fileType, userId: userId, groups: groups);

        // Assert
        result.Should().Be(expected: expectedPath);
    }

    [Test]
    public void Should_ReturnCorrectPathForReports()
    {
        // Arrange
        const int fileType = 2;
        string userId = Guid.NewGuid().ToString();

        string expectedPath =
            $"RootPath/{userId}/{Core.Application.Shared.Services.Acl.Disk.DirectoryInfoService.Reports}/20220101/group1/group2";

        string[] groups =
        [
            "group1",
            "group2"
        ];

        _directoryInfoServiceMock
            .Setup(expression: x => x.GetReportsDirectory(userId, groups, It.IsAny<string>()))
            .Returns(value: expectedPath);

        // Act
        string result = TestCandidate.ResolvePath(fileType: fileType, userId: userId, groups: groups);

        // Assert
        result.Should().Be(expected: expectedPath);
    }
}