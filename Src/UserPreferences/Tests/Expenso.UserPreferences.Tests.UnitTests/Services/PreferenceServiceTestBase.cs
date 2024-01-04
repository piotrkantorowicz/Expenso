using Expenso.Shared.MessageBroker;
using Expenso.Shared.UserContext;
using Expenso.UserPreferences.Core.Models;
using Expenso.UserPreferences.Core.Repositories;
using Expenso.UserPreferences.Core.Services;
using Expenso.UserPreferences.Core.Validators;

namespace Expenso.UserPreferences.Tests.UnitTests.Services;

internal abstract class PreferenceServiceTestBase : TestBase
{
    protected IPreferencesService TestCandidate { get; private set; } = null!;

    protected Mock<IPreferencesRepository> PreferencesRepositoryMock { get; private set; } = null!;

    protected Mock<IMessageBroker> MessageBrokerMock { get; private set; } = null!;

    protected Mock<IPreferenceValidator> PreferenceValidatorMock { get; private set; } = null!;

    protected Mock<IUserContextAccessor> UserContextAccessorMock { get; private set; } = null!;

    protected Mock<IUserContext> UserContextMock { get; private set; } = null!;

    protected Preference Preference { get; private set; } = null!;

    protected Guid UserId { get; private set; }

    [SetUp]
    public void SetUp()
    {
        UserId = Guid.NewGuid();
        Preference = Preference.CreateDefault(UserId);
        PreferencesRepositoryMock = new Mock<IPreferencesRepository>();
        MessageBrokerMock = new Mock<IMessageBroker>();
        UserContextAccessorMock = new Mock<IUserContextAccessor>();
        PreferenceValidatorMock = new Mock<IPreferenceValidator>();
        UserContextMock = new Mock<IUserContext>();
        UserContextMock.Setup(x => x.UserId).Returns(UserId.ToString());

        TestCandidate = new PreferencesService(PreferencesRepositoryMock.Object, UserContextAccessorMock.Object,
            MessageBrokerMock.Object, PreferenceValidatorMock.Object);
    }
}