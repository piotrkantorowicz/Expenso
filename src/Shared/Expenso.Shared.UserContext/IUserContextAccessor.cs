namespace Expenso.Shared.UserContext;

public interface IUserContextAccessor
{
    IUserContext? Get();
}