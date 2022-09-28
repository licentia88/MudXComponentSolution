namespace MudXComponents.Extensions;

public static class PropertyExtensions
{
    public static void SetPropertyValue(this object inputObject, string propertyName, object propertyValue)
    {
        var type = inputObject.GetType();

        var propInfo = type.GetProperty(propertyName);

        var propertyType = propInfo?.PropertyType;

        var targetType = IsNullable(propertyType) ? Nullable.GetUnderlyingType(propertyType!) : propertyType;

        //propertyValue =  Convert.ChangeType(propertyValue, targetType!);

        propInfo?.SetValue(inputObject, propertyValue, null);
    }

    public static object GetPropertyValue(this object obj, string propName)
    {
        return obj.GetType().GetProperty(propName)?.GetValue(obj);

    }
    private static bool IsNullable(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }

    public static bool HasMethod(this object objectToCheck, string methodName)
    {
        var type = objectToCheck.GetType();
        return type.GetMethod(methodName) != null;
    }

    public static bool HasProperty(this object objectToCheck, string propertyName)
    {
        var type = objectToCheck.GetType();
        return type.GetProperty(propertyName) != null;
    }

    //internal static (string, object)[] Add(this (string,object)[] parameters, (string, object) newParameter)
    //{
    //    return Add(parameters, newParameter);
    //}

    public static T[] AddParam<T>(this T[] parameters, T newParameter)
    {
        var result = new T[parameters.Length + 1];

        parameters[0] = newParameter;

        parameters.CopyTo(result, 1);



        return result;
    }

    public static IEnumerable<TType> Exclude<TType, ExcludeType>(this IEnumerable<TType> list)
    {
        return list.Where(x => x is not ExcludeType).Cast<TType>();
    }
}
