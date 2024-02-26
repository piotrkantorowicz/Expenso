using Expenso.BudgetSharing.Domain.Shared;
using Expenso.Shared.System.Types.Messages.Interfaces;

using Moq;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain;

public static class MessageContextFactoryResolverInitializer
{
    public static void Initialize(IMessageContextFactory messageContextFactory)
    {
        Mock<IServiceProvider> serviceProviderMock = new();
        serviceProviderMock.Setup(x => x.GetService(typeof(IMessageContextFactory))).Returns(messageContextFactory);
        MessageContextFactoryResolver.BindResolver(serviceProviderMock.Object);
    }
}