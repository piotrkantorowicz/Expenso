using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.IAM.Shared;
using Expenso.IAM.Shared.DTO.GetUserByEmail.Response;
using Expenso.Shared.System.Types.Clock;

using Moq;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.Services.
    AssignParticipantionDomainService;

[TestFixture]
internal abstract class AssignParticipantDomainServiceTestBase : DomainTestBase<IAssignParticipantionDomainService>
{
    [SetUp]
    public void SetUp()
    {
        _budgetPermissionRequestRepositoryMock = new Mock<IBudgetPermissionRequestRepository>();
        _budgetPermissionRepositoryMock = new Mock<IBudgetPermissionRepository>();
        _iamProxyMock = new Mock<IIamProxy>();
        _clockMock = new Mock<IClock>();

        _clockMock
            .Setup(expression: x => x.UtcNow)
            .Returns(value: new DateTimeOffset(year: 2024, month: 1, day: 1, hour: 0, minute: 0, second: 0,
                offset: TimeSpan.Zero));

        _participantId = PersonId.New(value: Guid.NewGuid());
        _budgetId = BudgetId.New(value: Guid.NewGuid());
        _ownerId = PersonId.New(value: Guid.NewGuid());

        _getUserByEmailResponse = new GetUserByEmailResponse(UserId: _participantId.Value.ToString(),
            Firstname: "Valentina", Lastname: "Long", Username: "vLong", Email: "email@email.com");

        _budgetPermission = BudgetPermission.Create(budgetId: _budgetId, ownerId: _ownerId);
        _email = _getUserByEmailResponse.Email;

        TestCandidate = new BudgetSharing.Domain.BudgetPermissionRequests.Services.AssignParticipantionDomainService(
            iamProxy: _iamProxyMock.Object,
            budgetPermissionRequestRepository: _budgetPermissionRequestRepositoryMock.Object, clock: _clockMock.Object,
            budgetPermissionRepository: _budgetPermissionRepositoryMock.Object);
    }

    protected const int ExpirationDays = 3;

    protected static readonly object[] PermissionTypes =
    [
        new object[]
        {
            PermissionType.SubOwner
        },
        new object[]
        {
            PermissionType.Reviewer
        },
        new object[]
        {
            PermissionType.Owner
        }
    ];

    protected static readonly object[] NoOwnerPermissionTypes =
    [
        new object[]
        {
            PermissionType.SubOwner
        },
        new object[]
        {
            PermissionType.Reviewer
        }
    ];

    protected readonly PermissionType _permissionType = PermissionType.SubOwner;
    protected BudgetId _budgetId = null!;
    protected BudgetPermission _budgetPermission = null!;
    protected Mock<IBudgetPermissionRepository> _budgetPermissionRepositoryMock = null!;
    protected Mock<IBudgetPermissionRequestRepository> _budgetPermissionRequestRepositoryMock = null!;
    protected Mock<IClock> _clockMock = null!;
    protected string _email = null!;
    protected GetUserByEmailResponse _getUserByEmailResponse = null!;
    protected Mock<IIamProxy> _iamProxyMock = null!;
    protected PersonId _ownerId = null!;
    protected PersonId _participantId = null!;
}