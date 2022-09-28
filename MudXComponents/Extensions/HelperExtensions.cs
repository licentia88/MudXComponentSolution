using System.Collections.ObjectModel;
using System.Text.Json;

namespace MudXComponents.Extensions;

public static class HelperExtensions
{
    /// <summary>
    /// Converts IEnumerable<T> to ObservableCollection<T>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerableList"></param>
    /// <returns></returns>
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerableList)
    {
        return enumerableList != null ? new ObservableCollection<T>(enumerableList) : new();
    }

    /// <summary>
    /// Converts objects to Immutable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T ToImmutable<T>(this T obj)
    {
        var stringified = JsonSerializer.Serialize(obj);

        return JsonSerializer.Deserialize<T>(stringified)!;
    }

    /// <summary>
    /// Checks wheter object is null or not
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    /// <param name="argument"></param>
    /// <returns></returns>
    public static bool IsNullOrDefault<TObject>(this TObject argument)
    {
        // deal with normal scenarios
        if (argument == null)
        {
            return true;
        }

        // deal with non-null nullables
        var methodType = typeof(TObject);
        if (Nullable.GetUnderlyingType(methodType) != null)
        {
            return false;
        }

        // deal with boxed value types
        var argumentType = argument.GetType();
        if (!argumentType.IsValueType || argumentType == methodType) return false;
        var obj = Activator.CreateInstance(argument.GetType());

        return obj.Equals(argument);
    }
}
