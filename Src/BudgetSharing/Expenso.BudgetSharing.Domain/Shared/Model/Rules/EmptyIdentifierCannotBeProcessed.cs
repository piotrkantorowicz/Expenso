using System.Reflection;
using System.Text;

using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.Shared.Model.Rules;

internal sealed class EmptyIdentifierCannotBeProcessed(Guid identifier, MemberInfo? type = null) : IBusinessRule
{
    public string Message
    {
        get
        {
            StringBuilder stringBuilder = new();
            stringBuilder.Append("Empty identifier");

            if (type != null)
            {
                stringBuilder.Append(" - ").Append(type.Name);
            }

            return stringBuilder.Append(" cannot be processed").ToString();
        }
    }

    public bool IsBroken()
    {
        return identifier == Guid.Empty;
    }
}