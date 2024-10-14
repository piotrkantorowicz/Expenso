namespace Expenso.Shared.System.Types.TypesExtensions;

public static class EnumExtensions
{
    public static TDestination? SafeCast<TDestination, TSource>(this TSource? value, TDestination? defaultValue = null)
        where TDestination : struct, Enum where TSource : struct, Enum
    {
        if (value == null)
        {
            return defaultValue;
        }

        int underlyingValue = Convert.ToInt32(value: value);

        return Enum.IsDefined(enumType: typeof(TDestination), value: underlyingValue)
            ? (TDestination)(object)underlyingValue
            : defaultValue;
    }
}