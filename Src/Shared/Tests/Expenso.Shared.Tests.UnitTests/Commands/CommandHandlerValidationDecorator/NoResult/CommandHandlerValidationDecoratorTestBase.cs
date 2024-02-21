using Expenso.Shared.Commands;
using Expenso.Shared.Commands.Validation;
using Expenso.Shared.Tests.UnitTests.Commands.TestData.NoResult;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandlerValidationDecorator.NoResult;

internal abstract class
    CommandHandlerValidationDecoratorTestBase : TestBase<CommandHandlerValidationDecorator<TestCommand>>
{
    protected TestCommand _testCommand = null!;
    protected Mock<ICommandValidator<TestCommand>> _validator = null!;

    [SetUp]
    protected void Setup()
    {
        _testCommand = new TestCommand(MessageContextFactoryMock.Object.Current(), Guid.NewGuid(), "JYi9R7e7v2Qor");
        _validator = new Mock<ICommandValidator<TestCommand>>();
        Mock<ICommandHandler<TestCommand>> handler = new();
        TestCandidate = new CommandHandlerValidationDecorator<TestCommand>([_validator.Object], handler.Object);
    }
}