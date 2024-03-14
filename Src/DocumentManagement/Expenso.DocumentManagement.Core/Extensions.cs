using System.IO.Abstractions;

using Expenso.DocumentManagement.Core.Application.Proxy;
using Expenso.DocumentManagement.Core.Application.Shared.Services;
using Expenso.DocumentManagement.Core.Application.Shared.Services.Acl.Disk;
using Expenso.DocumentManagement.Proxy;
using Expenso.Shared.System.Configuration.Extensions;
using Expenso.Shared.System.Configuration.Sections;
using Expenso.Shared.System.Configuration.Settings.Files;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expenso.DocumentManagement.Core;

public static class Extensions
{
    public static void AddDocumentManagementCore(this IServiceCollection services, IConfiguration configuration)
    {
        RegisterAclUserServices(services, configuration);
        services.AddScoped<IDocumentManagementProxy, DocumentManagementProxy>();
    }

    private static void RegisterAclUserServices(IServiceCollection services, IConfiguration configuration)
    {
        configuration.TryBindOptions(SectionNames.Files, out FilesSettings? filesSettings);

        switch (filesSettings?.StorageType)
        {
            case FileStorageType.Disk:
                services.AddScoped<IFileStorage, FileStorage>();
                services.AddScoped<IDirectoryPathResolver, DirectoryPathResolver>();
                services.AddScoped<IDirectoryInfoService, DirectoryInfoService>();
                services.AddScoped<IFileSystem, FileSystem>();

                break;
            default:
                throw new ArgumentOutOfRangeException(filesSettings?.StorageType.GetType().Name,
                    filesSettings?.StorageType, "Invalid auth server type.");
        }
    }
}