using Expenso.Shared.Integration.MessageBroker;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Factories;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Write.UpdatePreference.
    UpdatePreferenceCommandHandler;

[TestFixture]
internal abstract class UpdatePreferenceCommandHandlerTestBase : TestBase<
    Core.Application.Preferences.Write.Commands.UpdatePreference.UpdatePreferenceCommandHandler>
{
    [SetUp]
    public void SetUp()
    {
        _id = Guid.NewGuid();
        _userId = Guid.NewGuid();
        _preference = PreferenceFactory.Create(userId: _userId);
        _preferenceRepositoryMock = new Mock<IPreferencesRepository>();
        _messageBrokerMock = new Mock<IMessageBroker>();

        TestCandidate = new Core.Application.Preferences.Write.Commands.UpdatePreference.UpdatePreferenceCommandHandler(
            preferencesRepository: _preferenceRepositoryMock.Object,
            messageBroker: _messageBrokerMock.Object, messageContextFactory: MessageContextFactoryMock.Object);
    }

    protected Mock<IMessageBroker> _messageBrokerMock = null!;
    protected Preference _preference = null!;
    protected Mock<IPreferencesRepository> _preferenceRepositoryMock = null!;
    protected Guid _userId;
    protected Guid _id;
}