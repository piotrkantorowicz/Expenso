﻿using Expenso.Shared.System.Configuration.Settings.App;
using Expenso.Shared.System.Logging;

using Microsoft.Extensions.Logging;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.System.Logging.LoggerService;

[TestFixture]
internal abstract class LoggerServiceTestBase : TestBase<LoggerService<LoggerServiceTestBase>>
{
    [SetUp]
    public void SetUp()
    {
        _loggerFactoryMock = new Mock<ILoggerFactory>();
        _loggerMock = new Mock<ILogger<LoggerService<object>>>();

        _applicationSettings = new ApplicationSettings
        {
            InstanceId = Guid.NewGuid(),
            Name = "Test Application",
            Version = "1.0.0"
        };

        _loggerFactoryMock
            .Setup(expression: factory => factory.CreateLogger(It.IsAny<string>()))
            .Returns(value: _loggerMock.Object);

        TestCandidate = new LoggerService<LoggerServiceTestBase>(logger: _loggerFactoryMock.Object,
            applicationSettings: _applicationSettings);
    }

    private ApplicationSettings? _applicationSettings;
    private Mock<ILoggerFactory>? _loggerFactoryMock;
    protected Mock<ILogger<LoggerService<object>>>? _loggerMock;
}