using Expenso.Api.Configuration.Settings.Services.Validators.EfCore;
using Expenso.Shared.Database.EfCore.Settings;
using Expenso.Shared.Tests.Utils.UnitTests;

using FluentValidation;

using Moq;

using NUnit.Framework;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.EfCore.EfCoreValidator;

[TestFixture]
internal abstract class EfCoreSettingsValidatorTestBase : TestBase<EfCoreSettingsValidator>
{
    [SetUp]
    public void SetUp()
    {
        _efCoreSettings = new EfCoreSettings
        {
            ConnectionParameters = new ConnectionParameters
            {
                Host = "valid-host.com",
                Port = "5432",
                DefaultDatabase = "ValidDefaultDB",
                Database = "ValidDB",
                User = "ValidUser",
                Password = "ValidPassword1!"
            },
            InMemory = true,
            UseMigration = true,
            UseSeeding = true
        };

        Mock<IValidator<ConnectionParameters>> connectionParametersValidatorMock = new();

        TestCandidate = new EfCoreSettingsValidator(
            connectionParametersValidator: connectionParametersValidatorMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _efCoreSettings = null!;
        TestCandidate = null!;
    }

    protected EfCoreSettings _efCoreSettings = null!;
}