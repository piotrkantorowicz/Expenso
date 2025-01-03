using Expenso.Api.Configuration.Settings.ApiSettings.TimeZone;
using Expenso.Api.Configuration.Settings.Services.Validators;
using Expenso.Shared.System.Time.Constants;
using Expenso.Shared.Tests.Utils.UnitTests;

using NUnit.Framework;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.TimeZoneValidator;

internal abstract class TimeZoneSettingsValidatorTestBase : TestBase<TimeZoneSettingsValidator>
{
    [SetUp]
    public void SetUp()
    {
        _timeZoneSettings = new TimeZoneSettings
        {
            Id = "Utc",
            EnableRequestToUtc = true,
            EnableResponseToLocal = true,
            SupportedDateTimeFormats = DateTimeFormats.SupportedDateTimeFormats,
            SupportedDateTimeOffsetFormats = DateTimeFormats.SupportedDateTimeOffsetFormats,
            TimeZoneProviderType = TimeZoneProviderType.All
        };

        TestCandidate = new TimeZoneSettingsValidator();
    }

    [TearDown]
    public void TearDown()
    {
        _timeZoneSettings = null!;
        TestCandidate = null!;
    }

    protected TimeZoneSettings _timeZoneSettings = null!;
}