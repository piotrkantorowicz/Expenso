using Expenso.Shared.MessageBroker;
using Expenso.UserPreferences.Core.Application.Preferences.Commands.UpdatePreference;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Commands.UpdatePreference;

internal abstract class UpdatePreferenceCommandHandlerTestBase : TestBase<UpdatePreferenceCommandHandler>
{
    protected Guid _id;
    protected Mock<IMessageBroker> _messageBrokerMock = null!;
    protected Preference _preference = null!;
    protected Mock<IPreferencesRepository> _preferenceRepositoryMock = null!;
    protected Guid _userId;

    [SetUp]
    public void SetUp()
    {
        _id = Guid.NewGuid();
        _userId = Guid.NewGuid();
        _preference = Preference.CreateDefault(_id, _userId);
        _preferenceRepositoryMock = new Mock<IPreferencesRepository>();
        _messageBrokerMock = new Mock<IMessageBroker>();
        TestCandidate = new UpdatePreferenceCommandHandler(_preferenceRepositoryMock.Object, _messageBrokerMock.Object);
    }
}