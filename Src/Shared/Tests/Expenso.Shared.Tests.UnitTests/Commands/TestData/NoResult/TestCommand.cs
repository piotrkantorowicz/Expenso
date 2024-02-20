using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.Shared.Tests.UnitTests.Commands.TestData.NoResult;

internal sealed record TestCommand(IMessageContext MessageContext, Guid Id, string Name) : ICommand;