using Expenso.Shared.MessageBroker;
using Expenso.Shared.UserContext;
using Expenso.UserPreferences.Core.Models;
using Expenso.UserPreferences.Core.Repositories;
using Expenso.UserPreferences.Core.Services;
using Expenso.UserPreferences.Core.Validators;

namespace Expenso.UserPreferences.Tests.UnitTests.Services;

internal abstract class PreferenceServiceTestBase : TestBase<IPreferencesService>
{
    private Mock<IMessageBroker> _messageBrokerMock = null!;
    protected Mock<IPreferencesRepository> _preferencesRepositoryMock = null!;
    protected Mock<IPreferenceValidator> _preferenceValidatorMock = null!;
    protected Mock<IUserContextAccessor> _userContextAccessorMock = null!;
    protected Mock<IUserContext> _userContextMock = null!;
    protected Preference _preference = null!;
    protected Guid _userId = Guid.NewGuid();
    protected Guid _preferenceId = Guid.NewGuid();

    [SetUp]
    public void SetUp()
    {
        _preference = Preference.CreateDefault(_userId);
        _preferencesRepositoryMock = new Mock<IPreferencesRepository>();
        _messageBrokerMock = new Mock<IMessageBroker>();
        _userContextAccessorMock = new Mock<IUserContextAccessor>();
        _preferenceValidatorMock = new Mock<IPreferenceValidator>();
        _userContextMock = new Mock<IUserContext>();
        _userContextMock.Setup(x => x.UserId).Returns(_userId.ToString());

        TestCandidate = new PreferencesService(_preferencesRepositoryMock.Object, _userContextAccessorMock.Object,
            _messageBrokerMock.Object, _preferenceValidatorMock.Object);
    }
}