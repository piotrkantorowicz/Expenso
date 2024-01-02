using Expenso.UserPreferences.Core.Data.Ef;
using Expenso.UserPreferences.Core.Data.Ef.Repositories;
using Expenso.UserPreferences.Core.Models;
using Expenso.UserPreferences.Core.Repositories;

using Microsoft.EntityFrameworkCore;

using MockQueryable.Moq;

namespace Expenso.UserPreferences.Tests.UnitTests.Repositories;

internal abstract class PreferenceRepositoryTestBase : TestBase
{
    private IList<Preference> _preferences = null!;

    protected IPreferencesRepository TestCandidate { get; private set; } = null!;

    protected Mock<DbSet<Preference>> PreferenceDbSetMock { get; private set; } = null!;

    protected static IList<Guid> PreferenceIds =>
    [
        new Guid("19967114-32ef-4202-90c8-3aa590d14a03"),
        new Guid("87ddf365-e001-4949-abae-451d7ccd46c1"),
        new Guid("d3b1e36e-f188-4858-8d07-1b8bcd1b87fb")
    ];

    protected static IList<Guid> UserIds =>
    [
        new Guid("527336da-3371-45a9-9b9f-bbd42d01ffc2"),
        new Guid("3318e89e-fe27-453b-b9cb-3edce39ee187"),
        new Guid("41e0197a-014f-419a-9521-d0946e88818d")
    ];

    protected IReadOnlyCollection<Preference> Preferences => _preferences.AsReadOnly();

    private Mock<IUserPreferencesDbContext> DbContextMock { get; set; } = null!;

    [SetUp]
    public void Setup()
    {
        _preferences =
        [
            Preference.CreateDefault(PreferenceIds[0], UserIds[0]),
            Preference.Create(PreferenceIds[1], UserIds[1], GeneralPreference.Create(true),
                FinancePreference.Create(true, 2, false, 0), NotificationPreference.Create(true, 3)),
            Preference.Create(PreferenceIds[2], UserIds[2], GeneralPreference.Create(true),
                FinancePreference.Create(true, 3, true, 5), NotificationPreference.Create(true, 10))
        ];

        PreferenceDbSetMock = _preferences.AsQueryable().BuildMockDbSet();
        DbContextMock = new Mock<IUserPreferencesDbContext>();
        DbContextMock.Setup(x => x.Preferences).Returns(PreferenceDbSetMock.Object);
        TestCandidate = new PreferencesRepository(DbContextMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        PreferenceDbSetMock.Reset();
        DbContextMock.Reset();
        TestCandidate = null!;
        _preferences.Clear();
    }

    protected void AddPreference(Preference preference)
    {
        _preferences.Add(preference);
    }
}