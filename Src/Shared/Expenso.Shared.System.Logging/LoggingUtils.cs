using Microsoft.Extensions.Logging;

namespace Expenso.Shared.System.Logging;

public static class LoggingUtils
{
    public static EventId CommandExecuting { get; } = new(2000, "CommandExecuting");

    public static EventId CommandExecuted { get; } = new(2001, "CommandExecuted");

    public static EventId QueryExecuting { get; } = new(2010, "QueryExecuting");

    public static EventId QueryExecuted { get; } = new(2011, "QueryExecuted");

    public static EventId DomainEventExecuting { get; } = new(2020, "DomainEventExecuting");

    public static EventId DomainEventExecuted { get; } = new(2021, "DomainEventExecuted");

    public static EventId IntegrationEventExecuting { get; } = new(2030, "IntegrationEventExecuting");

    public static EventId IntegrationEventExecuted { get; } = new(2031, "IntegrationEventExecuted");

    public static EventId UnexpectedException { get; } = new(2100, "UnexpectedException");
}