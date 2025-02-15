using Expenso.Shared.System.Time.Providers.Interfaces;

namespace Expenso.Shared.System.Time.Features.Interfaces;

public interface IRequestFeature
{
    IRequestProvider? Provider { get; }
}