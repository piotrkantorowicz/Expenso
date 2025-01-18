using Expenso.IAM.Core.Application.Users.Read.Queries.GetUsers.DTO.Maps;
using Expenso.IAM.Core.Application.Users.Read.Services;
using Expenso.IAM.Shared.DTO.GetUsers.Response;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.Shared.System.Types.Paging;
using Expenso.Shared.System.Types.Paging.Constants;
using Expenso.Shared.Tests.Utils.UnitTests;

using Keycloak.AuthServices.Sdk.Admin.Models;

using Moq;

using NUnit.Framework;

namespace Expenso.IAM.Tests.UnitTests.Users.Queries.GetUser.GetUsersQueryHandler;

[TestFixture]
internal abstract class
    GetUsersQueryHandlerTestBase : TestBase<Core.Application.Users.Read.Queries.GetUsers.GetUsersQueryHandler>
{
    [SetUp]
    public void SetUp()
    {
        _userId = Guid.NewGuid().ToString();

        IEnumerable<UserRepresentation> users =
        [
            new()
            {
                Id = _userId,
                FirstName = "Valentina",
                LastName = "Long",
                Username = "vLong",
                Email = "email@email.com"
            },
            new()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Brenda",
                LastName = "Mai",
                Username = "BrendaMai",
                Email = "mai@email.com"
            }
        ];

        _defaultPagination = new Pagination(Page: PaginationDefaults.Page, Limit: PaginationDefaults.Limit);
        _getUsersResponse = GetUsersResponseMap.MapTo(users: users, pagination: _defaultPagination);
        _userServiceMock = new Mock<IUserService>();
        _messageContextMock = new Mock<IMessageContext>();

        TestCandidate =
            new Core.Application.Users.Read.Queries.GetUsers.GetUsersQueryHandler(userService: _userServiceMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _messageContextMock.Reset();
        _userServiceMock.Reset();
        _getUsersResponse = null!;
        _messageContextMock = null!;
        _userServiceMock = null!;
        _userId = null!;
        TestCandidate = null!;
    }

    protected IPagedList<GetUsersResponse> _getUsersResponse = null!;
    protected Mock<IMessageContext> _messageContextMock = null!;
    protected Mock<IUserService> _userServiceMock = null!;
    protected Pagination _defaultPagination = null!;
    private string _userId = null!;
}