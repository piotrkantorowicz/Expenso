using Expenso.DocumentManagement.Core.Application.Files.Write.AddFiles.DTO.Request;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.DocumentManagement.Core.Application.Files.Write.AddFiles;

public sealed record AddFilesCommand(IMessageContext MessageContext, AddFilesRequest AddFilesRequest) : ICommand;