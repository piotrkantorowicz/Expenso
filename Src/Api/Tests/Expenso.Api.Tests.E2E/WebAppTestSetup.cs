namespace Expenso.Api.Tests.E2E;

[SetUpFixture]
internal sealed class WebAppTestSetup
{
    [OneTimeSetUp]
    public void OneTimeSetup()
    {
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        WebApp.Instance.Destroy();
    }
}