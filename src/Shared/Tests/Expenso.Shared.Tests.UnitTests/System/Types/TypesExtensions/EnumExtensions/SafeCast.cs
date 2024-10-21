using Expenso.Shared.System.Types.TypesExtensions;
using Expenso.Shared.Tests.UnitTests.System.Types.TypesExtensions.EnumExtensions.TestObjects;

namespace Expenso.Shared.Tests.UnitTests.System.Types.TypesExtensions.EnumExtensions;

[TestFixture]
internal sealed class SafeCast
{
    [Test]
    public void Should_ReturnDefaultValue_When_SourceIsNull()
    {
        SourceEnum? source = null;
        DestinationEnum? result = source.SafeCast<DestinationEnum, SourceEnum>(defaultValue: DestinationEnum.Value3);
        result.Should().Be(expected: DestinationEnum.Value3);
    }

    [Test]
    public void Should_ReturnCorrespondingValue_When_SourceIsDefinedInDestinationEnum()
    {
        SourceEnum? source = SourceEnum.Value1;
        DestinationEnum? result = source.SafeCast<DestinationEnum, SourceEnum>();
        result.Should().Be(expected: DestinationEnum.Value1);
    }

    [Test]
    public void Should_ReturnDefaultValue_When_SourceIsNotDefinedInDestinationEnum()
    {
        SourceEnum? source = (SourceEnum)99;
        DestinationEnum? result = source.SafeCast<DestinationEnum, SourceEnum>(defaultValue: DestinationEnum.Value3);
        result.Should().Be(expected: DestinationEnum.Value3);
    }

    [Test]
    public void Should_ReturnNull_When_SourceIsNotDefinedInDestinationEnumAndNoDefaultValueProvided()
    {
        SourceEnum? source = (SourceEnum)99;
        DestinationEnum? result = source.SafeCast<DestinationEnum, SourceEnum>();
        result.Should().BeNull();
    }
}