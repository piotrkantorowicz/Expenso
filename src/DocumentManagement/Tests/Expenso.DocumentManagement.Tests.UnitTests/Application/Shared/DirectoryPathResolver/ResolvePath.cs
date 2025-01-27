using Expenso.DocumentManagement.Core.Application.Shared.Exceptions;
using Expenso.DocumentManagement.Core.Application.Shared.Models;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.DocumentManagement.Tests.UnitTests.Application.Shared.DirectoryPathResolver;

[TestFixture]
internal sealed class ResolvePath : DirectoryPathResolverTestBase
{
    [Test]
    public void Should_ReturnCorrectPathForImports()
    {
        // Arrange
        const FileType fileType = FileType.Import;
        string userId = Guid.CreateVersion7().ToString();

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
        result.ShouldBe(expected: expectedPath);
    }

    [Test]
    public void Should_ReturnCorrectPathForReports()
    {
        // Arrange
        const FileType fileType = FileType.Report;
        string userId = Guid.CreateVersion7().ToString();

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
        result.ShouldBe(expected: expectedPath);
    }

    [Test]
    public void Should_ThrowInvalidFileTypeException()
    {
        // Arrange
        const FileType fileType = FileType.None;
        string userId = Guid.CreateVersion7().ToString();
        string[] groups = [];

        // Act
        Action action = () => TestCandidate.ResolvePath(fileType: fileType, userId: userId, groups: groups);

        // Assert
        action.ShouldThrow<InvalidFileTypeException>();
    }
}