﻿using System.Reflection;

using Expenso.Communication.Api;
using Expenso.Communication.Shared;
using Expenso.Communication.Tests.UnitTests.Application.Notifications.Factories.NotificationServiceFactory;

using Extensions = Expenso.Communication.Core.Extensions;

namespace Expenso.Communication.Tests.ArchTests;

internal static class Assemblies
{
    private static readonly Assembly Api = typeof(CommunicationModule).Assembly;
    private static readonly Assembly Core = typeof(Extensions).Assembly;
    private static readonly Assembly Proxy = typeof(ICommunicationProxy).Assembly;
    private static readonly Assembly UnitTests = typeof(NotificationServiceFactoryTestBase).Assembly;
    private static readonly Assembly ArchTests = typeof(Assemblies).Assembly;

    public static IReadOnlyCollection<Assembly> GetAssemblies()
    {
        List<Assembly> assemblies =
        [
            Api,
            Core,
            Proxy,
            UnitTests,
            ArchTests
        ];

        return assemblies;
    }
}