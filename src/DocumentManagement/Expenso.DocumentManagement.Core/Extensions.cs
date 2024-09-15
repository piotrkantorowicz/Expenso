using System.IO.Abstractions;

using Expenso.DocumentManagement.Core.Application.Shared.Services;
using Expenso.DocumentManagement.Core.Application.Shared.Services.Acl.Disk;
using Expenso.Shared.System.Configuration;
using Expenso.Shared.System.Configuration.Sections;
using Expenso.Shared.System.Configuration.Settings.Files;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expenso.DocumentManagement.Core;

public static class Extensions
{
    public static void AddDocumentManagementCore(this IServiceCollection services, IConfiguration configuration)
    {
        RegisterAclUserServices(services: services, configuration: configuration);
    }

    private static void RegisterAclUserServices(IServiceCollection services, IConfiguration configuration)
    {
        configuration.TryBindOptions(sectionName: SectionNames.Files, options: out FilesSettings? filesSettings);

        switch (filesSettings?.StorageType)
        {
            case FileStorageType.Disk:
                services.AddScoped<IFileStorage, FileStorage>();
                services.AddScoped<IDirectoryPathResolver, DirectoryPathResolver>();
                services.AddScoped<IDirectoryInfoService, DirectoryInfoService>();
                services.AddScoped<IFileSystem, FileSystem>();

                break;
            default:
                throw new ArgumentOutOfRangeException(paramName: filesSettings?.StorageType.GetType().Name,
                    actualValue: filesSettings?.StorageType, message: "Invalid auth server type");
        }
    }
}