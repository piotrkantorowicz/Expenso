using Expenso.Shared.System.Time.Constants;
using Expenso.Shared.System.Time.Request;

using Microsoft.AspNetCore.Mvc.ModelBinding;

using Moq;

using NUnit.Framework;

namespace Expenso.Shared.Tests.UnitTests.System.Time.ModelBinders.DateTimeModelBinder;

internal abstract class DateTimeModelBinderTestBase
{
    protected Shared.System.Time.ModelBinders.DateTimeModelBinder _binder;
    protected Mock<IValueProvider> _valueProviderMock;
    protected ModelBindingContext _bindingContext;
    protected Mock<Func<RequestTimeZone>> _requestTimeZone;

    [SetUp]
    public void SetUp()
    {
        _requestTimeZone = new Mock<Func<RequestTimeZone>>();

        _requestTimeZone
            .Setup(expression: f => f())
            .Returns(value: new RequestTimeZone(timeZoneInfo: TimeZoneInfo.Utc));

        _binder = new Shared.System.Time.ModelBinders.DateTimeModelBinder(requestTimeZone: _requestTimeZone.Object,
            dateTimeSupportedFormats: DateTimeFormats.SupportedDateTimeFormats,
            dateTimeOffsetSupportedFormats: DateTimeFormats.SupportedDateTimeOffsetFormats);

        _valueProviderMock = new Mock<IValueProvider>();
        ValueProviderResult valueProviderResult = new(values: "2023-12-25T00:00:00Z");
        _valueProviderMock.Setup(expression: v => v.GetValue(It.IsAny<string>())).Returns(value: valueProviderResult);

        _bindingContext = new DefaultModelBindingContext
        {
            ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(modelType: typeof(DateTime?)),
            ModelName = "test",
            ModelState = [],
            ValueProvider = _valueProviderMock.Object
        };
    }
}