using Expenso.IAM.Core.Application.Users.Read.Queries.GetUserByEmail.DTO.Maps;
using Expenso.IAM.Core.Application.Users.Read.Services;
using Expenso.IAM.Shared.DTO.GetUserByEmail.Response;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.Shared.Tests.Utils.UnitTests;

using Keycloak.AuthServices.Sdk.Admin.Models;

using Moq;

using NUnit.Framework;

namespace Expenso.IAM.Tests.UnitTests.Users.Queries.GetUser.GetUserByEmailQueryHandler;

[TestFixture]
internal abstract class
    GetUserByEmailQueryHandlerTestBase : TestBase<
    Core.Application.Users.Read.Queries.GetUserByEmail.GetUserByEmailQueryHandler>
{
    [SetUp]
    public void SetUp()
    {
        _userEmail = "email@email.com";

        UserRepresentation user = new()
        {
            Id = Guid.CreateVersion7().ToString(),
            FirstName = "Valentina",
            LastName = "Long",
            Username = "vLong",
            Email = "email@email.com"
        };

        _getUserByEmailResponse = GetUserByEmailResponseMap.MapTo(user: user);
        _userServiceMock = new Mock<IUserService>();
        _messageContextMock = new Mock<IMessageContext>();

        TestCandidate =
            new Core.Application.Users.Read.Queries.GetUserByEmail.GetUserByEmailQueryHandler(
                userService: _userServiceMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _messageContextMock.Reset();
        _userServiceMock.Reset();
        _userEmail = null!;
        _getUserByEmailResponse = null!;
        _messageContextMock = null!;
        _userServiceMock = null!;
        TestCandidate = null!;
    }

    protected GetUserByEmailResponse _getUserByEmailResponse = null!;
    protected Mock<IMessageContext> _messageContextMock = null!;
    protected string _userEmail = null!;
    protected Mock<IUserService> _userServiceMock = null!;
}