using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.Shared.Tests.UnitTests.Commands.TestData.Result;

internal sealed record TestCommand(IMessageContext MessageContext, Guid Id, string? Payload) : ICommand;