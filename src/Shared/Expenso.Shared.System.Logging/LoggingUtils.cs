using Microsoft.Extensions.Logging;

namespace Expenso.Shared.System.Logging;

public static class LoggingUtils
{
    public static EventId GeneralInformation { get; } = new(id: 2000, name: "GeneralInformation");

    public static EventId CommandExecuting { get; } = new(id: 2100, name: "CommandExecuting");

    public static EventId CommandExecuted { get; } = new(id: 2101, name: "CommandExecuted");

    public static EventId QueryExecuting { get; } = new(id: 2200, name: "QueryExecuting");

    public static EventId QueryExecuted { get; } = new(id: 2201, name: "QueryExecuted");

    public static EventId DomainEventExecuting { get; } = new(id: 2300, name: "DomainEventExecuting");

    public static EventId DomainEventExecuted { get; } = new(id: 2301, name: "DomainEventExecuted");

    public static EventId IntegrationEventExecuting { get; } = new(id: 2400, name: "IntegrationEventExecuting");

    public static EventId IntegrationEventExecuted { get; } = new(id: 2401, name: "IntegrationEventExecuted");

    public static EventId BackgroundJobGeneralInformation { get; } =
        new(id: 2500, name: "BackgroundJobGeneralInformation");

    public static EventId BackgroundJobStarting { get; } = new(id: 2501, name: "BackgroundJobStarting");

    public static EventId BackgroundJobFinished { get; } = new(id: 2502, name: "BackgroundJobFinished");

    public static EventId GeneralWarning { get; } = new(id: 4000, name: "GeneralWarning");

    public static EventId SerializationWarning { get; } = new(id: 4001, name: "SerializationWarning");

    public static EventId BackgroundJobWarning { get; } = new(id: 4002, name: "BackgroundJobProcessingWarning");

    public static EventId UnexpectedError { get; } = new(id: 5000, name: "UnexpectedError");

    public static EventId BackgroundJobError { get; } = new(id: 5001, name: "BackgroundJobError");
}