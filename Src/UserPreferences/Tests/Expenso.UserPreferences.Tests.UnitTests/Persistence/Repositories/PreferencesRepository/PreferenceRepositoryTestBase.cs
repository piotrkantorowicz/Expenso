using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Core.Persistence.EfCore;

using Microsoft.EntityFrameworkCore;

using MockQueryable.Moq;

using TestCandidate = Expenso.UserPreferences.Core.Persistence.EfCore.Repositories.PreferencesRepository;

namespace Expenso.UserPreferences.Tests.UnitTests.Persistence.Repositories.PreferencesRepository;

internal abstract class PreferenceRepositoryTestBase : TestBase<IPreferencesRepository>
{
    protected static IList<PreferenceId> _preferenceIds =
    [
        PreferenceId.New(new Guid("19967114-32ef-4202-90c8-3aa590d14a03")),
        PreferenceId.New(new Guid("87ddf365-e001-4949-abae-451d7ccd46c1")),
            PreferenceId.New(new Guid("d3b1e36e-f188-4858-8d07-1b8bcd1b87fb"))
    ];

    protected static IList<UserId> _userIds =
    [
        UserId.New(new Guid("527336da-3371-45a9-9b9f-bbd42d01ffc2")),
        UserId.New(new Guid("3318e89e-fe27-453b-b9cb-3edce39ee187")),
            UserId.New(new Guid("41e0197a-014f-419a-9521-d0946e88818d"))
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
            Preference.CreateDefault(_preferenceIds[0], _userIds[0]),
            Preference.Create(_preferenceIds[1], _userIds[1], GeneralPreference.Create(true),
                FinancePreference.Create(true, 2, false, 0), NotificationPreference.Create(true, 3)),
            Preference.Create(_preferenceIds[2], _userIds[2], GeneralPreference.Create(true),
                FinancePreference.Create(true, 3, true, 5), NotificationPreference.Create(true, 10))
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