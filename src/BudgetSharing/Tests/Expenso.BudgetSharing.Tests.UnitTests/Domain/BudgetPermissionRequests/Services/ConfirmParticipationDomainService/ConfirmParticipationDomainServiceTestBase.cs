using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.System.Types.Clock;
using Expenso.UserPreferences.Shared;
using Expenso.UserPreferences.Shared.DTO.API.GetPreference.Response;

using Moq;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.Services.
    ConfirmParticipationDomainService;

[TestFixture]
internal abstract class ConfirmParticipationDomainServiceTestBase : DomainTestBase<IConfirmParticipantionDomainService>
{
    [SetUp]
    public void SetUp()
    {
        _budgetPermissionRepositoryMock = new Mock<IBudgetPermissionRepository>();
        _budgetPermissionRequestRepositoryMock = new Mock<IBudgetPermissionRequestRepository>();
        _userPreferencesProxyMock = new Mock<IUserPreferencesProxy>();
        _clockMock = new Mock<IClock>();

        _clockMock
            .Setup(expression: x => x.UtcNow)
            .Returns(value: new DateTimeOffset(year: 2024, month: 1, day: 1, hour: 0, minute: 0, second: 0,
                offset: TimeSpan.Zero));

        _budgetPermissionRequest = BudgetPermissionRequest.Create(budgetId: BudgetId.New(value: Guid.NewGuid()),
            personId: PersonId.New(value: Guid.NewGuid()), ownerId: PersonId.New(value: Guid.NewGuid()),
            permissionType: PermissionType.SubOwner, expirationDate: _clockMock.Object.UtcNow.AddDays(days: 3),
            clock: _clockMock.Object);

        PersonId ownerId = PersonId.New(value: Guid.NewGuid());
        _budgetPermissionRequestId = _budgetPermissionRequest.Id;
        _budgetId = _budgetPermissionRequest.BudgetId;
        _budgetPermission = BudgetPermission.Create(budgetId: _budgetPermissionRequest.BudgetId, ownerId: ownerId);
        _budgetPermission.AddPermission(participantId: ownerId, permissionType: PermissionType.Owner);

        _getPreferenceResponse = new GetPreferencesResponse(Id: Guid.NewGuid(), UserId: ownerId.Value,
            FinancePreference: new GetPreferencesResponse_FinancePreference(AllowAddFinancePlanSubOwners: true,
                MaxNumberOfSubFinancePlanSubOwners: 1, AllowAddFinancePlanReviewers: true,
                MaxNumberOfFinancePlanReviewers: 3), NotificationPreference: null, GeneralPreference: null);

        TestCandidate = new ConfirmParticipantionDomainService(
            budgetPermissionRequestRepository: _budgetPermissionRequestRepositoryMock.Object,
            budgetPermissionRepository: _budgetPermissionRepositoryMock.Object,
            userPreferencesProxy: _userPreferencesProxyMock.Object, clock: _clockMock.Object);

        // clear uncommitted changes
        _budgetPermissionRequest.GetUncommittedChanges();
        _budgetPermission.GetUncommittedChanges();
    }

    protected BudgetId _budgetId = null!;
    protected BudgetPermission _budgetPermission = null!;
    protected Mock<IBudgetPermissionRepository> _budgetPermissionRepositoryMock = null!;
    protected BudgetPermissionRequest _budgetPermissionRequest = null!;
    protected BudgetPermissionRequestId _budgetPermissionRequestId = null!;
    protected Mock<IBudgetPermissionRequestRepository> _budgetPermissionRequestRepositoryMock = null!;
    private Mock<IClock> _clockMock = null!;
    protected GetPreferencesResponse _getPreferenceResponse = null!;
    protected Mock<IUserPreferencesProxy> _userPreferencesProxyMock = null!;
}