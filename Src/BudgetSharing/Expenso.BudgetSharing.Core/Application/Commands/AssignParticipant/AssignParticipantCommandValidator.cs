using Expenso.Shared.Commands.Validations;

namespace Expenso.BudgetSharing.Core.Application.Commands.AssignParticipant;

internal sealed class AssignParticipantCommandValidator : ICommandValidator<AssignParticipantCommand>
{
    public IDictionary<string, string> Validate(AssignParticipantCommand command)
    {
        throw new NotImplementedException();
    }
}