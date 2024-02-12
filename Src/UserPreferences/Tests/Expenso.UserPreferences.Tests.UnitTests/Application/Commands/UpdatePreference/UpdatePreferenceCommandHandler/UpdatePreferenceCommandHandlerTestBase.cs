using Expenso.Shared.MessageBroker;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;

using TestCandidate =
    Expenso.UserPreferences.Core.Application.Preferences.Commands.UpdatePreference.UpdatePreferenceCommandHandler;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Commands.UpdatePreference.UpdatePreferenceCommandHandler;

internal abstract class UpdatePreferenceCommandHandlerTestBase : TestBase<TestCandidate>
{
    private PreferenceId _id = null!;
    protected Mock<IMessageBroker> _messageBrokerMock = null!;
    protected Preference _preference = null!;
    protected Mock<IPreferencesRepository> _preferenceRepositoryMock = null!;
    protected UserId _userId = null!;

    [SetUp]
    public void SetUp()
    {
        _id = PreferenceId.New(Guid.NewGuid());
        _userId = UserId.New(Guid.NewGuid());
        _preference = Preference.CreateDefault(_id, _userId);
        _preferenceRepositoryMock = new Mock<IPreferencesRepository>();
        _messageBrokerMock = new Mock<IMessageBroker>();
        TestCandidate = new TestCandidate(_preferenceRepositoryMock.Object, _messageBrokerMock.Object);
    }
}