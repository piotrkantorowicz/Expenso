using Expenso.Api.Configuration.Settings.Services.Validators;
using Expenso.Shared.System.Configuration.Settings.Files;
using Expenso.Shared.Tests.Utils.UnitTests;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.FileSettingsValidator;

[TestFixture]
internal abstract class FileSettingsValidatorTestBase : TestBase<FilesSettingsValidator>
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

        TestCandidate = new FilesSettingsValidator();
    }

    protected FilesSettings _filesSettings = null!;
}