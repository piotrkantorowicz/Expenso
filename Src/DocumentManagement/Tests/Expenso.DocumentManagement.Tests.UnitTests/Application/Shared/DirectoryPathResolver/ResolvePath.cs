using FluentAssertions;

using Moq;

using DirectoryInfo = Expenso.DocumentManagement.Core.Application.Shared.Services.Acl.Disk.DirectoryInfoService;

namespace Expenso.DocumentManagement.Tests.UnitTests.Application.Shared.DirectoryPathResolver;

internal sealed class ResolvePath : DirectoryPathResolverTestBase
{
    [Test]
    public void Should_ReturnCorrectPathForImports()
    {
        // Arrange
        const int fileType = 1;
        string userId = Guid.NewGuid().ToString();
        string expectedPath = $"RootPath/{userId}/{DirectoryInfo.Imports}/20220101/group1/group2";

        string[] groups =
        [
            "group1",
            "group2"
        ];

        _directoryInfoServiceMock
            .Setup(x => x.GetImportsDirectory(userId, groups, It.IsAny<string>()))
            .Returns(expectedPath);

        // Act
        string result = TestCandidate.ResolvePath(fileType, userId, groups);

        // Assert
        result.Should().Be(expectedPath);
    }

    [Test]
    public void Should_ReturnCorrectPathForReports()
    {
        // Arrange
        const int fileType = 2;
        string userId = Guid.NewGuid().ToString();
        string expectedPath = $"RootPath/{userId}/{DirectoryInfo.Reports}/20220101/group1/group2";

        string[] groups =
        [
            "group1",
            "group2"
        ];

        _directoryInfoServiceMock
            .Setup(x => x.GetReportsDirectory(userId, groups, It.IsAny<string>()))
            .Returns(expectedPath);

        // Act
        string result = TestCandidate.ResolvePath(fileType, userId, groups);

        // Assert
        result.Should().Be(expectedPath);
    }
}