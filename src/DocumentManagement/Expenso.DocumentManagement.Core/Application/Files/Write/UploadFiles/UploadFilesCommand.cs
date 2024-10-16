using Expenso.DocumentManagement.Shared.DTO.API.UploadFiles.Request;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.DocumentManagement.Core.Application.Files.Write.UploadFiles;

public sealed record UploadFilesCommand(IMessageContext MessageContext, UploadFilesRequest UploadFilesRequest)
    : ICommand;