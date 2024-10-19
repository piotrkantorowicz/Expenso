using Expenso.Api.Tests.E2E.TestData.IAM;
using Expenso.IAM.Shared;
using Expenso.IAM.Shared.DTO.GetUser.Response;

namespace Expenso.Api.Tests.E2E.IAM;

internal sealed class FakeIamProxy : IIamProxy
{
    public static readonly string[] ExistingEmails =
        ["MatthewSoto@email.com", "JorgePandey@email.com", "EiIbrahim@email.com"];

    private readonly IReadOnlyCollection<GetUserResponse> _users =
    [
        new(UserId: UserDataInitializer.UserIds[index: 0].ToString(), Firstname: "Sergio", Lastname: "Huang",
            Username: "SHuang", Email: ExistingEmails[0]),
        new(UserId: new Guid(g: "32b61237-4859-4281-8702-6fa3e4c72d67").ToString(), Firstname: "Krishna",
            Lastname: "Le", Username: "KLeee", Email: ExistingEmails[1]),
        new(UserId: new Guid(g: "0d53ecf2-cef4-47ca-974a-3f1b395cd2c4").ToString(), Firstname: "Vincent",
            Lastname: "Ashraf", Username: "VAshraf", Email: ExistingEmails[2])
    ];

    public async Task<GetUserResponse?> GetUserByIdAsync(string userId, CancellationToken cancellationToken)
    {
        return await Task.FromResult(result: _users.FirstOrDefault(predicate: x => x.UserId == userId));
    }

    public async Task<GetUserResponse?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await Task.FromResult(result: _users.FirstOrDefault(predicate: x => x.Email == email));
    }
}