using Expenso.IAM.Core.DTO;
using Expenso.IAM.Core.Mappings;
using Expenso.IAM.Core.ModuleApi;
using Expenso.IAM.Core.Services.Interfaces;
using Expenso.IAM.Proxy;
using Expenso.IAM.Proxy.Contracts;

namespace Expenso.IAM.Tests.UnitTests.Proxy;

internal abstract class IamApiTestBase : TestBase
{
    protected UserContract _userContract = null!;
    protected UserDto _userDto = null!;

    protected IIamApi TestCandidate { get; private set; } = null!;

    protected Mock<IUserService> UserServiceMock { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        _userDto = AutoFixtureProxy.Create<UserDto>();
        _userContract = UserDtoToUserContract.Map(_userDto)!;
        UserServiceMock = AutoFixtureProxy.Freeze<Mock<IUserService>>();
        TestCandidate = AutoFixtureProxy.Build<IamApi>().WithAutoProperties().Create();
    }
}