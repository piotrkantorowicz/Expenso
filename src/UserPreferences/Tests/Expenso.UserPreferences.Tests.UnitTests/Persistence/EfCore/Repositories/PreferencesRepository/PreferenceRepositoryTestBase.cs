using Expenso.Shared.Tests.Utils.UnitTests;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Factories;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Core.Persistence.EfCore;

using Microsoft.EntityFrameworkCore;

using MockQueryable.Moq;

using Moq;

using NUnit.Framework;

namespace Expenso.UserPreferences.Tests.UnitTests.Persistence.EfCore.Repositories.PreferencesRepository;

[TestFixture]
internal abstract class PreferenceRepositoryTestBase : TestBase<IPreferencesRepository>
{
    [SetUp]
    public void Setup()
    {
        _preferences =
        [
            PreferenceFactory.Create(userId: _userIds[index: 0], preferenceId: _preferenceIds[index: 0]),
            PreferenceFactory.Create(userId: _userIds[index: 1], preferenceId: _preferenceIds[index: 1]) with
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
            PreferenceFactory.Create(userId: _userIds[index: 2], preferenceId: _preferenceIds[index: 2]) with
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
        _dbContextMock.Setup(expression: x => x.Preferences).Returns(value: _preferenceDbSetMock.Object);

        TestCandidate =
            new Core.Persistence.EfCore.Repositories.PreferencesRepository(
                userPreferencesDbContext: _dbContextMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _preferenceDbSetMock.Reset();
        _dbContextMock.Reset();
        TestCandidate = null!;
        _preferences.Clear();
    }

    protected static IList<Guid> _preferenceIds =
    [
        new(g: "0194ba94-0f72-7dfc-a9b9-cad571f69f9d"),
        new(g: "0194ba94-0f72-72ee-8545-8bf71427663d"),
        new(g: "0194ba94-0f72-7c19-8930-a805438ff34b")
    ];

    protected static IList<Guid> _userIds =
    [
        new(g: "0194ba94-0f72-7023-8efc-8a33c7522cf5"),
        new(g: "0194ba94-0f72-7809-81d2-11a0b0a37715"),
        new(g: "0194ba94-0f72-7bed-b521-1d291a844ccb")
    ];

    private Mock<IUserPreferencesDbContext> _dbContextMock = null!;
    protected Mock<DbSet<Preference>> _preferenceDbSetMock = null!;
    private IList<Preference> _preferences = null!;

    protected IList<Preference> Preferences => _preferences.AsReadOnly();

    protected void AddPreference(Preference preference)
    {
        _preferences.Add(item: preference);
    }
}