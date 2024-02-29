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
    protected Mock<IClock> _clockMock = null!;
    protected const int ExpirationDays = 3;
    protected readonly PermissionType _permissionType = PermissionType.SubOwner;
    protected Mock<IIamProxy> _iamProxyMock = null!;
    protected PersonId _participantId = null!;
    private Mock<IBudgetPermissionRequestRepository> _budgetPermissionRequestRepositoryMock = null!;
    protected BudgetId _budgetId = null!;
    protected string _email = null!;
    protected GetUserResponse _getUserResponse = null!;

    [SetUp]
    public void SetUp()
    {
        _budgetPermissionRequestRepositoryMock = new Mock<IBudgetPermissionRequestRepository>();
        _iamProxyMock = new Mock<IIamProxy>();
        _clockMock = new Mock<IClock>();
        _clockMock.Setup(x => x.UtcNow).Returns(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero));
        _participantId = PersonId.New(Guid.NewGuid());
        _budgetId = BudgetId.New(Guid.NewGuid());

        _getUserResponse = new GetUserResponse(_participantId.Value.ToString(), "Valentina", "Long", "vLong",
            "email@email.com");

        _email = _getUserResponse.Email;

        TestCandidate = new TestCandidate(_iamProxyMock.Object, _budgetPermissionRequestRepositoryMock.Object,
            _clockMock.Object);
    }
}