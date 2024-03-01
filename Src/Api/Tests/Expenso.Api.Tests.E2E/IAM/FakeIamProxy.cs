using Expenso.Api.Tests.E2E.TestData.Preferences;
using Expenso.IAM.Proxy;
using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.Api.Tests.E2E.IAM;

internal sealed class FakeIamProxy : IIamProxy
{
    public static readonly string[] ExistingEmails =
        ["MatthewSoto@email.com", "JorgePandey@email.com", "EiIbrahim@email.com"];

    public Task<GetUserResponse?> GetUserByIdAsync(string userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<GetUserResponse?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return email switch
        {
            "MatthewSoto@email.com" => Task.FromResult<GetUserResponse?>(new GetUserResponse(
                PreferencesDataProvider.UserIds[0].ToString(), "Sergio", "Huang", "SHuang", ExistingEmails[0])),
            "JorgePandey@email.com" => Task.FromResult<GetUserResponse?>(new GetUserResponse(
                new Guid("32b61237-4859-4281-8702-6fa3e4c72d67").ToString(), "Krishna", "Le", "KLeee",
                ExistingEmails[1])),
            "EiIbrahim@email.com" => Task.FromResult<GetUserResponse?>(new GetUserResponse(
                new Guid("0d53ecf2-cef4-47ca-974a-3f1b395cd2c4").ToString(), "Vincent", "Ashraf", "VAshraf",
                ExistingEmails[2])),
            _ => throw new NotFoundException($"User with email {email} not found")
        };
    }
}