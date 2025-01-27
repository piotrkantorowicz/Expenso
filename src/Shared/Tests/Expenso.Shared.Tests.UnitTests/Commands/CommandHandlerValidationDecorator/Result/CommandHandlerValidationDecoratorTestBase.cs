using Expenso.Shared.Commands;
using Expenso.Shared.Commands.Validation;
using Expenso.Shared.Tests.UnitTests.Commands.TestData.Result;
using Expenso.Shared.Tests.Utils.UnitTests;

using FluentValidation;

using Moq;

using NUnit.Framework;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandlerValidationDecorator.Result;

[TestFixture]
internal abstract class
    CommandHandlerValidationDecoratorTestBase : TestBase<
    CommandHandlerValidationDecorator<TestCommand, TestCommandResult>>
{
    [SetUp]
    public void Setup()
    {
        _testCommand = new TestCommand(MessageContext: MessageContextFactoryMock.Object.Current(),
            Id: Guid.CreateVersion7(),
            Payload: "JYi9R7e7v2Qor");

        _validator = new Mock<IValidator<TestCommand>>();
        _handler = new Mock<ICommandHandler<TestCommand, TestCommandResult>>();

        TestCandidate = new CommandHandlerValidationDecorator<TestCommand, TestCommandResult>(
            validators: [_validator.Object], decorated: _handler.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _handler.Reset();
        _validator.Reset();
        _testCommand = null!;
        _handler = null!;
        _validator = null!;
        TestCandidate = null!;
    }

    protected Mock<ICommandHandler<TestCommand, TestCommandResult>> _handler = null!;
    protected TestCommand _testCommand = null!;
    protected Mock<IValidator<TestCommand>> _validator = null!;
}