using Expenso.Shared.Tests.Utils.UnitTests;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.DTO.Maps;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Factories;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Shared.DTO.API.CreatePreference.Response;

using Moq;

using NUnit.Framework;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Write.CreatePreference.
    CreatePreferenceCommandHandler;

[TestFixture]
internal abstract class CreatePreferenceCommandHandlerTestBase : TestBase<
    Core.Application.Preferences.Write.Commands.CreatePreference.CreatePreferenceCommandHandler>
{
    [SetUp]
    public void SetUp()
    {
        _userId = Guid.CreateVersion7();
        _preferenceId = Guid.CreateVersion7();
        _preference = PreferenceFactory.Create(preferenceId: _preferenceId, userId: _userId);
        _preferenceRepositoryMock = new Mock<IPreferencesRepository>();
        _createPreferenceResponse = CreatePreferenceResponseMap.MapTo(preference: _preference);

        TestCandidate =
            new Core.Application.Preferences.Write.Commands.CreatePreference.CreatePreferenceCommandHandler(
                preferencesRepository: _preferenceRepositoryMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _preferenceRepositoryMock.Reset();
        _preferenceRepositoryMock = null!;
        _createPreferenceResponse = null!;
        _preference = null!;
        TestCandidate = null!;
    }

    protected CreatePreferenceResponse _createPreferenceResponse = null!;
    protected Preference _preference = null!;
    protected Mock<IPreferencesRepository> _preferenceRepositoryMock = null!;
    protected Guid _userId;
    protected Guid _preferenceId;
}