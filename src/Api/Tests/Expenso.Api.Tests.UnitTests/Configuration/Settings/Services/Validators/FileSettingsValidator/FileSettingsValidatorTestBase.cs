using Expenso.Shared.System.Configuration.Settings.Files;
using Expenso.Shared.Tests.Utils.UnitTests;

using TestCandidate = Expenso.Api.Configuration.Settings.Services.Validators.FilesSettingsValidator;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.FileSettingsValidator;

[TestFixture]
internal abstract class FileSettingsValidatorTestBase : TestBase<TestCandidate>
{
    [SetUp]
    public void SetUp()
    {
        _filesSettings = new FilesSettings
        {
            StorageType = FileStorageType.Disk,
            ImportDirectory = "Import",
            ReportsDirectory = "Reports"
        };

        TestCandidate = new TestCandidate();
    }

    protected FilesSettings _filesSettings = null!;
}