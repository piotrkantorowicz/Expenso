using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.IAM.Proxy;
using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.System.Types.Clock;

using Moq;

using TestCandidate = Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.AssignParticipantDomainService;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.Services.AssignParticipantDomainService;

internal abstract class AssignParticipantDomainServiceTestBase : DomainTestBase<TestCandidate>
{
    protected const int ExpirationDays = 3;
    protected readonly PermissionType _permissionType = PermissionType.SubOwner;
    protected BudgetId _budgetId = null!;
    private Mock<IBudgetPermissionRequestRepository> _budgetPermissionRequestRepositoryMock = null!;
    protected Mock<IClock> _clockMock = null!;
    protected string _email = null!;
    protected GetUserResponse _getUserResponse = null!;
    protected Mock<IIamProxy> _iamProxyMock = null!;
    protected PersonId _participantId = null!;

    [SetUp]
    public void SetUp()
    {
        _budgetPermissionRequestRepositoryMock = new Mock<IBudgetPermissionRequestRepository>();
        _iamProxyMock = new Mock<IIamProxy>();
        _clockMock = new Mock<IClock>();

        _clockMock
            .Setup(expression: x => x.UtcNow)
            .Returns(value: new DateTimeOffset(year: 2024, month: 1, day: 1, hour: 0, minute: 0, second: 0,
                offset: TimeSpan.Zero));

        _participantId = PersonId.New(value: Guid.NewGuid());
        _budgetId = BudgetId.New(value: Guid.NewGuid());

        _getUserResponse = new GetUserResponse(UserId: _participantId.Value.ToString(), Firstname: "Valentina",
            Lastname: "Long", Username: "vLong", Email: "email@email.com");

        _email = _getUserResponse.Email;

        TestCandidate = new TestCandidate(iamProxy: _iamProxyMock.Object,
            budgetPermissionRequestRepository: _budgetPermissionRequestRepositoryMock.Object, clock: _clockMock.Object);
    }
}