using Expenso.Shared.Integration.MessageBroker;
using Expenso.Shared.Tests.Utils.UnitTests;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Factories;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;

using Moq;

using NUnit.Framework;

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
        _preferenceId = Guid.NewGuid();
        _preference = PreferenceFactory.Create(preferenceId: _preferenceId, userId: _userId);
        _preferenceRepositoryMock = new Mock<IPreferencesRepository>();
        _messageBrokerMock = new Mock<IMessageBroker>();

        TestCandidate = new Core.Application.Preferences.Write.Commands.UpdatePreference.UpdatePreferenceCommandHandler(
            preferencesRepository: _preferenceRepositoryMock.Object, messageBroker: _messageBrokerMock.Object,
            messageContextFactory: MessageContextFactoryMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _preferenceRepositoryMock.Reset();
        _messageBrokerMock.Reset();
        _preferenceRepositoryMock = null!;
        _messageBrokerMock = null!;
        TestCandidate = null!;
    }

    protected Mock<IMessageBroker> _messageBrokerMock = null!;
    protected Preference _preference = null!;
    protected Mock<IPreferencesRepository> _preferenceRepositoryMock = null!;
    protected Guid _id;
    private Guid _userId;
    private Guid _preferenceId;
}