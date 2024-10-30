using Expenso.Api.Configuration.Settings.Services.Validators;
using Expenso.Shared.Tests.Utils.UnitTests;
using Expenso.TimeManagement.Core.Application.Shared.Settings;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.TimeManagement;

[TestFixture]
internal abstract class TimeManagementSettingsValidatorTestBase : TestBase<TimeManagementSettingsValidator>
{
    [SetUp]
    public void SetUp()
    {
        _timeManagementSettings = new TimeManagementSettings
        {
            AllowedEvents = [AllowedEventType.BudgetPermissionRequestExpired]
        };

        TestCandidate = new TimeManagementSettingsValidator();
    }

    protected TimeManagementSettings _timeManagementSettings = null!;
}