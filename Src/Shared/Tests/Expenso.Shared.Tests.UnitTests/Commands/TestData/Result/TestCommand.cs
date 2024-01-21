using Expenso.Shared.Commands;

namespace Expenso.Shared.Tests.UnitTests.Commands.TestData.Result;

internal sealed record TestCommand(Guid Id, string Name) : ICommand;