using Expenso.Shared.Commands;

namespace Expenso.Shared.Tests.UnitTests.Commands.TestData.NoResult;

internal sealed record TestCommand(Guid Id, string Name) : ICommand;