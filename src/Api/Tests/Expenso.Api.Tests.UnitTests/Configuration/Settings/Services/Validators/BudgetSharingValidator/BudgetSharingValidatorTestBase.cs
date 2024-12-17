using Expenso.Api.Configuration.Settings.Services.Validators;
using Expenso.Shared.Tests.Utils.UnitTests;

using NUnit.Framework;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.BudgetSharingValidator;

[TestFixture]
internal abstract class BudgetSharingValidatorTestBase : TestBase<BudgetSharingSettingsValidator>
{
    [SetUp]
    public void SetUp()
    {
        TestCandidate = new BudgetSharingSettingsValidator();
    }

    [TearDown]
    public void TearDown()
    {
        TestCandidate = null!;
    }
}