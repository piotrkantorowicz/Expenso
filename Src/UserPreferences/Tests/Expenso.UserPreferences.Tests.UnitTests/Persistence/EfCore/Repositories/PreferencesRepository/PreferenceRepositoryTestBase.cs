using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Shared.Factories;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Core.Persistence.EfCore;

using Microsoft.EntityFrameworkCore;

using MockQueryable.Moq;

using TestCandidate = Expenso.UserPreferences.Core.Persistence.EfCore.Repositories.PreferencesRepository;

namespace Expenso.UserPreferences.Tests.UnitTests.Persistence.EfCore.Repositories.PreferencesRepository;

internal abstract class PreferenceRepositoryTestBase : TestBase<IPreferencesRepository>
{
    protected static IList<Guid> _preferenceIds =
    [
        new Guid("19967114-32ef-4202-90c8-3aa590d14a03"),
        new Guid("87ddf365-e001-4949-abae-451d7ccd46c1"),
        new Guid("d3b1e36e-f188-4858-8d07-1b8bcd1b87fb")
    ];

    protected static IList<Guid> _userIds =
    [
        new Guid("527336da-3371-45a9-9b9f-bbd42d01ffc2"),
        new Guid("3318e89e-fe27-453b-b9cb-3edce39ee187"),
        new Guid("41e0197a-014f-419a-9521-d0946e88818d")
    ];

    private Mock<IUserPreferencesDbContext> _dbContextMock = null!;
    protected Mock<DbSet<Preference>> _preferenceDbSetMock = null!;
    private IList<Preference> _preferences = null!;

    protected IEnumerable<Preference> Preferences => _preferences.AsReadOnly();

    [SetUp]
    public void Setup()
    {
        _preferences =
        [
            PreferenceFactory.Create(_userIds[0], _preferenceIds[0]),
            PreferenceFactory.Create(_userIds[1], _preferenceIds[1]) with
            {
                GeneralPreference = new GeneralPreference
                {
                    UseDarkMode = true
                },
                FinancePreference = new FinancePreference
                {
                    AllowAddFinancePlanSubOwners = true,
                    MaxNumberOfSubFinancePlanSubOwners = 2,
                    AllowAddFinancePlanReviewers = false,
                    MaxNumberOfFinancePlanReviewers = 0
                },
                NotificationPreference = new NotificationPreference
                {
                    SendFinanceReportEnabled = true,
                    SendFinanceReportInterval = 3
                }
            },
            PreferenceFactory.Create(_userIds[2], _preferenceIds[2]) with
            {
                GeneralPreference = new GeneralPreference
                {
                    UseDarkMode = true
                },
                FinancePreference = new FinancePreference
                {
                    AllowAddFinancePlanSubOwners = true,
                    MaxNumberOfSubFinancePlanSubOwners = 3,
                    AllowAddFinancePlanReviewers = true,
                    MaxNumberOfFinancePlanReviewers = 5
                },
                NotificationPreference = new NotificationPreference
                {
                    SendFinanceReportEnabled = true,
                    SendFinanceReportInterval = 10
                }
            }
        ];

        _preferenceDbSetMock = _preferences.AsQueryable().BuildMockDbSet();
        _dbContextMock = new Mock<IUserPreferencesDbContext>();
        _dbContextMock.Setup(x => x.Preferences).Returns(_preferenceDbSetMock.Object);
        TestCandidate = new TestCandidate(_dbContextMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _preferenceDbSetMock.Reset();
        _dbContextMock.Reset();
        TestCandidate = null!;
        _preferences.Clear();
    }

    protected void AddPreference(Preference preference)
    {
        _preferences.Add(preference);
    }
}