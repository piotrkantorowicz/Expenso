using System.Reflection;

namespace Expenso.Shared.Types.Domain;

public abstract record Enumeration<T>(int Value, string DisplayName) : IComparable<T> where T : Enumeration<T>
{
    private static readonly Lazy<Dictionary<int, T>> AllItems;
    private static readonly Lazy<Dictionary<string, T>> AllItemsByName;

    static Enumeration()
    {
        AllItems = new Lazy<Dictionary<int, T>>(() => typeof(T)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(x => x.FieldType == typeof(T))
            .Select(x => x.GetValue(null))
            .Cast<T>()
            .ToDictionary(x => x.Value, x => x));

        AllItemsByName = new Lazy<Dictionary<string, T>>(() =>
        {
            Dictionary<string, T> items = new(AllItems.Value.Count);

            foreach (KeyValuePair<int, T> item in AllItems.Value)
            {
                if (!items.TryAdd(item.Value.DisplayName, item.Value))
                {
                    throw new InvalidOperationException(
                        $"The display name '{item.Value.DisplayName}' has already been added to the enumeration.");
                }
            }

            return items;
        });
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
        return Math.Abs(firstValue.Value - secondValue.Value);
    }

    public static T FromValue(int value)
    {
        if (AllItems.Value.TryGetValue(value, out T? matchingItem))
        {
            return matchingItem;
        }

        throw new InvalidOperationException($"'{value}' is not a valid value in {typeof(T)}");
    }

    public static T FromDisplayName(string displayName)
    {
        if (AllItemsByName.Value.TryGetValue(displayName, out T? matchingItem))
        {
            return matchingItem;
        }

        throw new InvalidOperationException($"'{displayName}' is not a valid display name in {typeof(T)}");
    }

    public int CompareTo(T? other)
    {
        return Value.CompareTo(other!.Value);
    }
}