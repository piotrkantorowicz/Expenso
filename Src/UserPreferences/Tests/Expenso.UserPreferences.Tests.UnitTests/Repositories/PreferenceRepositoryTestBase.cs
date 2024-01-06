using Expenso.UserPreferences.Core.Data.Ef;
using Expenso.UserPreferences.Core.Data.Ef.Repositories;
using Expenso.UserPreferences.Core.Models;
using Expenso.UserPreferences.Core.Repositories;

using Microsoft.EntityFrameworkCore;

using MockQueryable.Moq;

namespace Expenso.UserPreferences.Tests.UnitTests.Repositories;

internal abstract class PreferenceRepositoryTestBase : TestBase<IPreferencesRepository>
{
    private IList<Preference> _preferences = null!;
    private Mock<IUserPreferencesDbContext> _dbContextMock = null!;

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

    protected Mock<DbSet<Preference>> _preferenceDbSetMock = null!;

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
        TestCandidate = new PreferencesRepository(_dbContextMock.Object);
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