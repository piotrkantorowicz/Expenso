using Expenso.Shared.Commands.Validation;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob;

internal sealed class RegisterJobCommandValidator : ICommandValidator<RegisterJobCommand>
{
    public IDictionary<string, string> Validate(RegisterJobCommand command)
    {
        throw new NotImplementedException();
    }
}