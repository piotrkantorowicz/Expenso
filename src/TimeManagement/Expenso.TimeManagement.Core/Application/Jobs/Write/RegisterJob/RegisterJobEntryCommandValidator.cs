using Expenso.Shared.Commands.Validation;
using Expenso.Shared.Commands.Validation.Utils;
using Expenso.Shared.System.Serialization;
using Expenso.Shared.System.Types.Clock;

using NCrontab;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob;

internal sealed class RegisterJobEntryCommandValidator : ICommandValidator<RegisterJobEntryCommand>
{
    private readonly IClock _clock;
    private readonly ISerializer _serializer;

    public RegisterJobEntryCommandValidator(ISerializer serializer, IClock clock)
    {
        _serializer = serializer ?? throw new ArgumentNullException(paramName: nameof(serializer));
        _clock = clock ?? throw new ArgumentNullException(paramName: nameof(clock));
    }

    private readonly IReadOnlyDictionary<string, CommandValidationRule<RegisterJobEntryCommand>[]> _validationMetadata =
        new Dictionary<string, CommandValidationRule<RegisterJobEntryCommand>[]>
        {
            {
                ValidationUtils.Command, [
                    new CommandValidationRule<RegisterJobEntryCommand>(validationFailedFunc: command => command is null,
                        createMessageFunc: _ => "Command is required.", validationType: ValidationTypes.Required,
                        value: true)
                ]
            },
            {
                nameof(RegisterJobEntryCommand.RegisterJobEntryRequest), [
                    new CommandValidationRule<RegisterJobEntryCommand>(
                        validationFailedFunc: command => command?.RegisterJobEntryRequest is null,
                        createMessageFunc: _ => "Register job entry request is required.",
                        validationType: ValidationTypes.Required, value: true)
                ]
            },
            {
                $"{nameof(RegisterJobEntryCommand.RegisterJobEntryRequest.MaxRetries)}", [
                    new CommandValidationRule<RegisterJobEntryCommand>(
                        validationFailedFunc: command => command?.RegisterJobEntryRequest?.MaxRetries is not null &&
                                                         command?.RegisterJobEntryRequest?.MaxRetries <= 0,
                        createMessageFunc: _ => "MaxRetries must be a positive value.",
                        validationType: ValidationTypes.Required, value: true)
                ]
            },
            {
                $"{nameof(RegisterJobEntryCommand.RegisterJobEntryRequest.Interval)}|{nameof(RegisterJobEntryCommand.RegisterJobEntryRequest.RunAt)}",
                [
                    new CommandValidationRule<RegisterJobEntryCommand>(
                        validationFailedFunc: command =>
                            command?.RegisterJobEntryRequest?.Interval is null &&
                            command.RegisterJobEntryRequest?.RunAt is null,
                        createMessageFunc: _ =>
                            "At least one value must be provided: Interval for periodic jobs or RunAt for single run jobs.",
                        validationType: ValidationTypes.Required, value: true)
                ]
            },
            {
                $"{nameof(RegisterJobEntryCommand.RegisterJobEntryRequest.Interval)}|{nameof(RegisterJobEntryCommand.RegisterJobEntryRequest.RunAt)}",
                [
                    new CommandValidationRule<RegisterJobEntryCommand>(
                        validationFailedFunc: command =>
                            command?.RegisterJobEntryRequest?.Interval is not null &&
                            command.RegisterJobEntryRequest?.RunAt is not null,
                        createMessageFunc: _ => "RunAt and Interval cannot be used together.",
                        validationType: ValidationTypes.Required, value: true)
                ]
            },
            {
                $"{nameof(RegisterJobEntryCommand.RegisterJobEntryRequest.Interval)}", [
                    new CommandValidationRule<RegisterJobEntryCommand>(validationFailedFunc: command =>
                    {
                        if (command?.RegisterJobEntryRequest?.Interval is null)
                        {
                            return false;
                        }

                        try
                        {
                            string cronExpression = command.RegisterJobEntryRequest.Interval.GetCronExpression();

                            CrontabSchedule.Parse(expression: cronExpression,
                                    options: new CrontabSchedule.ParseOptions()
                                ;

                            public IReadOnlyDictionary<string, CommandValidationRule<RegisterJobEntryCommand>[]>
                                GetValidationMetadata()
                            {
                                return new Dictionary<string, CommandValidationRule<RegisterJobEntryCommand>[]>();
                            }
                        }