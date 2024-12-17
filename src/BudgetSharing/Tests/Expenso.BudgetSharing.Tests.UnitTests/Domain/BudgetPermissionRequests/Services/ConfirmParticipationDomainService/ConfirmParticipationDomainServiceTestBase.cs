using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.System.Types.Clock;
using Expenso.UserPreferences.Shared;
using Expenso.UserPreferences.Shared.DTO.API.GetPreference.Response;

using Moq;

using NUnit.Framework;

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
        _budgetId = BudgetId.New(value: Guid.NewGuid());
        _budgetPermissionRequestId = BudgetPermissionRequestId.New(value: Guid.NewGuid());
        BudgetPermissionId budgetPermissionId = BudgetPermissionId.New(value: Guid.NewGuid());
        PersonId ownerId = PersonId.New(value: Guid.NewGuid());
        BudgetCode budgetCode = BudgetCode.New(value: "BDGT/11/12/2024");

        DateTimeOffset submissionDate = new(year: 2024, month: 1, day: 1, hour: 6, minute: 0, second: 0,
            offset: TimeSpan.Zero);

        _clockMock.Setup(expression: x => x.UtcNow).Returns(value: submissionDate);

        _budgetPermissionRequest = BudgetPermissionRequest.Create(budgetPermissionRequestId: _budgetPermissionRequestId,
            budgetId: _budgetId, personId: PersonId.New(value: Guid.NewGuid()), ownerId: ownerId,
            budgetCode: budgetCode, permissionType: PermissionType.SubOwner,
            expirationDate: _clockMock.Object.UtcNow.AddDays(days: 3), submissionDate: _clockMock.Object.UtcNow);

        _clockMock.Setup(expression: x => x.UtcNow).Returns(value: submissionDate.AddMinutes(minutes: 30));

        _budgetPermissionRepositoryMock
            .Setup(expression: x => x.IsUnique(budgetPermissionId, _budgetId, ownerId,
                budgetCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: true);

        _budgetPermission = BudgetPermission.Create(budgetPermissionId: budgetPermissionId,
            budgetId: _budgetPermissionRequest.BudgetId, ownerId: ownerId,
            budgetCode: _budgetPermissionRequest.BudgetCode,
            budgetPermissionRepository: _budgetPermissionRepositoryMock.Object);

        _budgetPermission.AddPermission(participantId: ownerId, permissionType: PermissionType.Owner);

        _getPreferenceResponse = new GetPreferencesResponse(Id: Guid.NewGuid(), UserId: ownerId.Value,
            FinancePreference: new GetPreferencesResponseFinancePreference(AllowAddFinancePlanSubOwners: true,
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

    [TearDown]
    public void TearDown()
    {
        _budgetPermissionRepositoryMock.Reset();
        _budgetPermissionRequestRepositoryMock.Reset();
        _userPreferencesProxyMock.Reset();
        _clockMock.Reset();
        _budgetPermissionRequest = null!;
        _budgetPermission = null!;
        _budgetPermissionRequestId = null!;
        _budgetId = null!;
        _budgetPermissionRepositoryMock = null!;
        _budgetPermissionRequestRepositoryMock = null!;
        _userPreferencesProxyMock = null!;
        _clockMock = null!;
        _budgetPermission = null!;
        _getPreferenceResponse = null!;
    }

    private Mock<IClock> _clockMock = null!;
    protected BudgetId _budgetId = null!;
    protected BudgetPermission _budgetPermission = null!;
    protected Mock<IBudgetPermissionRepository> _budgetPermissionRepositoryMock = null!;
    protected BudgetPermissionRequest _budgetPermissionRequest = null!;
    protected BudgetPermissionRequestId _budgetPermissionRequestId = null!;
    protected Mock<IBudgetPermissionRequestRepository> _budgetPermissionRequestRepositoryMock = null!;
    protected GetPreferencesResponse _getPreferenceResponse = null!;
    protected Mock<IUserPreferencesProxy> _userPreferencesProxyMock = null!;
}