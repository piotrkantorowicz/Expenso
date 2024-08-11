using Microsoft.Extensions.Logging;

namespace Expenso.Shared.System.Logging;

public static class LoggingUtils
{
    public static EventId CommandExecuting { get; } = new(id: 2000, name: "CommandExecuting");

    public static EventId CommandExecuted { get; } = new(id: 2001, name: "CommandExecuted");

    public static EventId QueryExecuting { get; } = new(id: 2010, name: "QueryExecuting");

    public static EventId QueryExecuted { get; } = new(id: 2011, name: "QueryExecuted");

    public static EventId DomainEventExecuting { get; } = new(id: 2020, name: "DomainEventExecuting");

    public static EventId DomainEventExecuted { get; } = new(id: 2021, name: "DomainEventExecuted");

    public static EventId IntegrationEventExecuting { get; } = new(id: 2030, name: "IntegrationEventExecuting");

    public static EventId IntegrationEventExecuted { get; } = new(id: 2031, name: "IntegrationEventExecuted");

    public static EventId UnexpectedException { get; } = new(id: 2100, name: "UnexpectedException");
}