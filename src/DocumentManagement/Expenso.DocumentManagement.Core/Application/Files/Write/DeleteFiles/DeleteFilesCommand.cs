using Expenso.DocumentManagement.Shared.DTO.API.DeleteFiles.Request;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.DocumentManagement.Core.Application.Files.Write.DeleteFiles;

public sealed record DeleteFilesCommand(IMessageContext MessageContext, DeleteFilesRequest? Payload)
    : ICommand;