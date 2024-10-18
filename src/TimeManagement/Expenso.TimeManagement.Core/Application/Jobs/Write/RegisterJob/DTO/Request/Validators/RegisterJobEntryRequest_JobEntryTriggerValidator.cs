using Expenso.Shared.System.Serialization;
using Expenso.Shared.System.Serialization.Default;
using Expenso.TimeManagement.Core.Application.Jobs.Shared.BackgroundJobs.Events;
using Expenso.TimeManagement.Shared.DTO.Request;

using FluentValidation;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob.DTO.Request.Validators;

internal sealed class
    RegisterJobEntryRequest_JobEntryTriggerValidator : AbstractValidator<RegisterJobEntryRequest_JobEntryTrigger>
{
    public RegisterJobEntryRequest_JobEntryTriggerValidator(ISerializer serializer,
        IEventTypeResolver eventTypeResolver)
    {
        ArgumentNullException.ThrowIfNull(argument: serializer, paramName: nameof(serializer));
        ArgumentNullException.ThrowIfNull(argument: eventTypeResolver, paramName: nameof(eventTypeResolver));
        RuleFor(expression: x => x.EventType).NotEmpty().WithMessage(errorMessage: "Event type is required.");
        RuleFor(expression: x => x.EventData).NotEmpty().WithMessage(errorMessage: "Event data is required.");

        When(predicate: x => x.EventType is not null && x.EventData is not null, action: () =>
        {
            RuleFor(expression: x => x)
                .Must(predicate: trigger =>
                {
                    if (!eventTypeResolver.IsAllowable(eventName: trigger.EventType!))
                    {
                        return false;
                    }

                    return serializer.Deserialize(value: trigger.EventData!,
                        type: eventTypeResolver.Resolve(eventName: trigger.EventType!),
                        settings: DefaultSerializerOptions.DefaultSettings) is not null;
                })
                .WithMessage(errorMessage: "EventData must be serializable to provided EventType.");
        });
    }
}