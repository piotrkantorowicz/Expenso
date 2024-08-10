using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Exceptions;

using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.Permissions;

internal sealed class Create : PermissionTestBase
{
    public static object[] PermissionTypes =
    {
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
    };

    [Test, TestCaseSource(sourceName: nameof(PermissionTypes))]
    public void Should_ReturnPermission_When_Created(PermissionType permissionType)
    {
        // Arrange
        PersonId participantId = PersonId.New(value: Guid.NewGuid());

        // Act
        Permission result = Permission.Create(participantId: participantId, permissionType: permissionType);

        // Assert
        result.Should().NotBeNull();
        result.ParticipantId.Should().Be(expected: participantId);
        result.PermissionType.Should().Be(expected: permissionType);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_PermissionTypeIsUnknown()
    {
        // Arrange
        PersonId participantId = PersonId.New(value: Guid.NewGuid());
        PermissionType permissionType = PermissionType.Unknown;

        // Act
        Action act = () => Permission.Create(participantId: participantId, permissionType: permissionType);

        // Assert
        act
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(
                expectedWildcardPattern: $"Unknown permission type {permissionType.Value} cannot be processed.");
    }
}