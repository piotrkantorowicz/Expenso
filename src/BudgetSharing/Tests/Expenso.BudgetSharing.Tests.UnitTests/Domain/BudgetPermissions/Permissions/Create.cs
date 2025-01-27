using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using NUnit.Framework;

using Shouldly;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.Permissions;

[TestFixture]
internal sealed class Create : PermissionTestBase
{
    public static object[] PermissionTypes =
    [
        new object[]
        {
            PermissionType.Owner
        },
        new object[]
        {
            PermissionType.SubOwner
        },
        new object[]
        {
            PermissionType.Reviewer
        }
    ];

    [Test, TestCaseSource(sourceName: nameof(PermissionTypes))]
    public void Should_ReturnPermission_When_Created(PermissionType permissionType)
    {
        // Arrange
        PersonId participantId = PersonId.New(value: Guid.CreateVersion7());

        // Act
        Permission result = Permission.Create(participantId: participantId, permissionType: permissionType);

        // Assert
        result.ShouldNotBeNull();
        result.ParticipantId.ShouldBe(expected: participantId);
        result.PermissionType.ShouldBe(expected: permissionType);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_PermissionTypeIsNone()
    {
        // Arrange
        PersonId participantId = PersonId.New(value: Guid.CreateVersion7());
        PermissionType permissionType = PermissionType.None;

        // Act
        Action action = () => Permission.Create(participantId: participantId, permissionType: permissionType);

        // Assert
        action.AssertDomainRuleValidationException(
            expectedDetails: $"Permission type cannot be empty for participant {participantId}.");
    }
}