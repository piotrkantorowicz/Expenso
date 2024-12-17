using Expenso.Shared.Tests.Utils.UnitTests;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Maps;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Response;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Factories;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;

using Moq;

using NUnit.Framework;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Read.Queries.GetPreference;

[TestFixture]
internal abstract class GetPreferenceQueryHandlerTestBase : TestBase<GetPreferenceQueryHandler>
{
    [SetUp]
    public void SetUp()
    {
        _preferenceId = Guid.NewGuid();
        _preference = PreferenceFactory.Create(preferenceId: _preferenceId, userId: Guid.NewGuid());
        _getPreferenceResponse = GetPreferenceResponseMap.MapTo(preference: _preference);
        _preferenceRepositoryMock = new Mock<IPreferencesRepository>();
        TestCandidate = new GetPreferenceQueryHandler(preferencesRepository: _preferenceRepositoryMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _preferenceRepositoryMock.Reset();
        _preferenceRepositoryMock = null!;
        _getPreferenceResponse = null!;
        _preference = null!;
        _preferenceId = default!;
        TestCandidate = null!;
    }

    protected GetPreferenceResponse _getPreferenceResponse = null!;
    protected Guid _preferenceId;
    protected Preference _preference = null!;
    protected Mock<IPreferencesRepository> _preferenceRepositoryMock = null!;
}