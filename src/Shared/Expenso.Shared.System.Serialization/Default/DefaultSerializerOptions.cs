using System.Text.Json;

using Expenso.Shared.System.Serialization.Converters;
using Expenso.Shared.System.Types.Messages;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.Shared.System.Serialization.Default;

public static class DefaultSerializerOptions
{
    public static readonly JsonSerializerOptions DefaultSettings = new()
    {
        Converters =
        {
            new InterfaceToConcreteTypeJsonConverter<IMessageContext, MessageContext>()
        }
    };
}