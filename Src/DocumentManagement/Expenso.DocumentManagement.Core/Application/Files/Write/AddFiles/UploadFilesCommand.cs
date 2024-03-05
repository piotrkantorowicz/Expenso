using Expenso.DocumentManagement.Proxy.DTO.API.UploadFiles.Request;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.DocumentManagement.Core.Application.Files.Write.AddFiles;

public sealed record UploadFilesCommand(IMessageContext MessageContext, UploadFilesRequest UploadFilesRequest)
    : ICommand;