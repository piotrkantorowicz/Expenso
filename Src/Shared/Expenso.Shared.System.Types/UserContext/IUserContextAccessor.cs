namespace Expenso.Shared.System.Types.UserContext;

public interface IUserContextAccessor
{
    IUserContext? Get();
}