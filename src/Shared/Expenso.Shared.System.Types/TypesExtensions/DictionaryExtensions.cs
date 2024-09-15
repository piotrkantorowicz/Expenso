namespace Expenso.Shared.System.Types.TypesExtensions;

public static class DictionaryExtensions
{
    public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IDictionary<TKey, TValue> items,
        bool overwrite = false)
    {
        ArgumentNullException.ThrowIfNull(argument: dictionary);
        ArgumentNullException.ThrowIfNull(argument: items);

        foreach (KeyValuePair<TKey, TValue> item in items)
        {
            if (dictionary.ContainsKey(key: item.Key))
            {
                if (overwrite)
                {
                    dictionary[key: item.Key] = item.Value;
                }
                else
                {
                    throw new InvalidOperationException(
                        message: $"Key '{item.Key}' already exists in the dictionary and overwrite is not allowed");
                }
            }
            else
            {
                dictionary.Add(key: item.Key, value: item.Value);
            }
        }
    }
}