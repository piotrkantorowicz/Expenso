using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.System.Types.Clock;
using Expenso.UserPreferences.Proxy;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Response;

using Moq;

using TestCandidate = Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.ConfirmParticipationDomainService;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.Services.
    ConfirmParticipationDomainService;

internal abstract class ConfirmParticipationDomainServiceTestBase : DomainTestBase<TestCandidate>
{
    protected BudgetId _budgetId = null!;
    protected BudgetPermission _budgetPermission = null!;
    protected Mock<IBudgetPermissionRepository> _budgetPermissionRepositoryMock = null!;
    protected BudgetPermissionRequest _budgetPermissionRequest = null!;
    protected BudgetPermissionRequestId _budgetPermissionRequestId = null!;
    protected Mock<IBudgetPermissionRequestRepository> _budgetPermissionRequestRepositoryMock = null!;
    private Mock<IClock> _clockMock = null!;
    protected GetPreferenceResponse _getPreferenceResponse = null!;
    protected Mock<IUserPreferencesProxy> _userPreferencesProxyMock = null!;

    [SetUp]
    public void SetUp()
    {
        _budgetPermissionRepositoryMock = new Mock<IBudgetPermissionRepository>();
        _budgetPermissionRequestRepositoryMock = new Mock<IBudgetPermissionRequestRepository>();
        _userPreferencesProxyMock = new Mock<IUserPreferencesProxy>();
        _clockMock = new Mock<IClock>();
        _clockMock.Setup(x => x.UtcNow).Returns(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero));

        _budgetPermissionRequest = BudgetPermissionRequest.Create(BudgetId.New(Guid.NewGuid()),
            PersonId.New(Guid.NewGuid()), PermissionType.SubOwner, 3, _clockMock.Object);

        PersonId ownerId = PersonId.New(Guid.NewGuid());
        _budgetPermissionRequestId = _budgetPermissionRequest.Id;
        _budgetId = _budgetPermissionRequest.BudgetId;
        _budgetPermission = BudgetPermission.Create(_budgetPermissionRequest.BudgetId, ownerId);
        _budgetPermission.AddPermission(ownerId, PermissionType.Owner);

        _getPreferenceResponse = new GetPreferenceResponse(Guid.NewGuid(), ownerId.Value,
            new GetPreferenceResponse_FinancePreference(true, 1, true, 3), null, null);

        TestCandidate = new TestCandidate(_budgetPermissionRequestRepositoryMock.Object,
            _budgetPermissionRepositoryMock.Object, _userPreferencesProxyMock.Object);

        // clear uncommitted changes
        _budgetPermissionRequest.GetUncommittedChanges();
        _budgetPermission.GetUncommittedChanges();
    }
}