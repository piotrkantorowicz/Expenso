using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.Shared.Queries;

public interface IQuery
{
    IMessageContext MessageContext { get; }
}

public interface IQuery<T> : IQuery;