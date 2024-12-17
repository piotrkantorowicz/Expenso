using Expenso.Shared.Commands;
using Expenso.Shared.Commands.Validation;
using Expenso.Shared.Tests.UnitTests.Commands.TestData.NoResult;
using Expenso.Shared.Tests.Utils.UnitTests;

using FluentValidation;

using Moq;

using NUnit.Framework;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandlerValidationDecorator.NoResult;

[TestFixture]
internal abstract class
    CommandHandlerValidationDecoratorTestBase : TestBase<CommandHandlerValidationDecorator<TestCommand>>
{
    [SetUp]
    public void Setup()
    {
        _testCommand = new TestCommand(MessageContext: MessageContextFactoryMock.Object.Current(), Id: Guid.NewGuid(),
            Payload: "JYi9R7e7v2Qor");

        _validator = new Mock<IValidator<TestCommand>>();
        Mock<ICommandHandler<TestCommand>> handler = new();

        TestCandidate = new CommandHandlerValidationDecorator<TestCommand>(validators: [_validator.Object],
            decorated: handler.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _validator.Reset();
        _testCommand = null!;
        _validator = null!;
        TestCandidate = null!;
    }

    protected TestCommand _testCommand = null!;
    protected Mock<IValidator<TestCommand>> _validator = null!;
}