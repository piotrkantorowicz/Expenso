using Expenso.BudgetSharing.Domain.BudgetPermissions;

using NUnit.Framework;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.Permissions;

[TestFixture]
internal abstract class PermissionTestBase : DomainTestBase<Permission>
{
    [SetUp]
    public void SetUp()
    {
    }

    [TearDown]
    public void TearDown()
    {
        TestCandidate = null!;
    }
}