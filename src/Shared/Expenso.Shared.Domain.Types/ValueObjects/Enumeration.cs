using System.Reflection;

namespace Expenso.Shared.Domain.Types.ValueObjects;

public abstract record Enumeration<T>(int Value, string DisplayName) : IComparable<T> where T : Enumeration<T>
{
    private static readonly Lazy<Dictionary<int, T>> AllItems;
    private static readonly Lazy<Dictionary<string, T>> AllItemsByName;

    static Enumeration()
    {
        AllItems = new Lazy<Dictionary<int, T>>(valueFactory: () => typeof(T)
            .GetFields(bindingAttr: BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(predicate: x => x.FieldType == typeof(T))
            .Select(selector: x => x.GetValue(obj: null))
            .Cast<T>()
            .ToDictionary(keySelector: x => x.Value, elementSelector: x => x));

        AllItemsByName = new Lazy<Dictionary<string, T>>(valueFactory: () =>
        {
            Dictionary<string, T> items = new(capacity: AllItems.Value.Count);

            foreach (KeyValuePair<int, T> item in AllItems.Value)
            {
                if (!items.TryAdd(key: item.Value.DisplayName, value: item.Value))
                {
                    throw new InvalidOperationException(
                        message:
                        $"The display name '{item.Value.DisplayName}' has already been added to the enumeration");
                }
            }

            return items;
        });
    }

    public int CompareTo(T? other)
    {
        return Value.CompareTo(value: other!.Value);
    }

    public override string ToString()
    {
        return DisplayName;
    }

    public static IEnumerable<T> GetAll()
    {
        return AllItems.Value.Values;
    }

    public static int AbsoluteDifference(Enumeration<T> firstValue, Enumeration<T> secondValue)
    {
        return Math.Abs(value: firstValue.Value - secondValue.Value);
    }

    public static T FromValue(int value)
    {
        if (AllItems.Value.TryGetValue(key: value, value: out T? matchingItem))
        {
            return matchingItem;
        }

        throw new InvalidOperationException(message: $"'{value}' is not a valid value in {typeof(T)}");
    }

    public static T FromDisplayName(string displayName)
    {
        if (AllItemsByName.Value.TryGetValue(key: displayName, value: out T? matchingItem))
        {
            return matchingItem;
        }

        throw new InvalidOperationException(message: $"'{displayName}' is not a valid display name in {typeof(T)}");
    }
}