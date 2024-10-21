namespace Expenso.IAM.Shared.DTO.GetUserById.Response;

public sealed record GetUserByIdResponse(
    string UserId,
    string? Firstname,
    string? Lastname,
    string Username,
    string Email)
{
    public string Fullname => $"{Firstname} {Lastname}";
}