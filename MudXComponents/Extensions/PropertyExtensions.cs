using System.ComponentModel.DataAnnotations.Schema;
using FastMember;

namespace MudXComponents.Extensions;

public static class PropertyExtensions
{
   
    public static void SetPropertyValue(this object inputObject, string propertyName, object propertyValue)
    {
        try
        {
            var type = inputObject.GetType();

            var accessor = TypeAccessor.Create(type);

            accessor[inputObject, propertyName] = propertyValue;

            //var propInfo = type.GetProperty(propertyName);

            //var propertyType = propInfo?.PropertyType;

            //var targetType = IsNullable(propertyType) ? Nullable.GetUnderlyingType(propertyType!) : propertyType;

            //propertyValue = Convert.ChangeType(propertyValue, targetType!);

            //propInfo?.SetValue(inputObject, propertyValue);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
   
    public static void SetPropertyValue<TModel,TBindingType>(this TModel inputObject, string propertyName, object propertyValue)
    {
        var type = typeof(TModel);

        var accessor = TypeAccessor.Create(type);

        accessor[inputObject, propertyName] = propertyValue;

        //not required until above code throws an exception


        //var propInfo = type.GetProperty(propertyName);

        //var propertyType = propInfo?.PropertyType;

        //object typedValue ;

        //TryParse(propertyValue, propertyType, out typedValue);


        //propInfo?.SetValue(inputObject, typedValue);
    }
    public static string GetForeignKey<TParent, TChild>() where TParent : class where TChild : class
    {
        var keyData = typeof(TParent).GetProperty(typeof(TChild).Name).CustomAttributes
            .FirstOrDefault(y => y.AttributeType == typeof(ForeignKeyAttribute))?.ConstructorArguments
            .FirstOrDefault().Value;

        if (keyData is null)
            return null;

        return keyData.ToString();
    }

    public static void SetParentChildRelation<TParent, TChild>(TParent parentModel, TChild childModel) where TParent : class where TChild : class
    {
        var primaryKey = typeof(TParent).GetKey();

        var foreignKey = GetForeignKey<TParent, TChild>();

        var primaryKeyValue = parentModel.GetType().GetProperty(primaryKey)?.GetValue(parentModel);

        childModel.GetType().GetProperty(foreignKey)?.SetValue(childModel, primaryKeyValue);
    }

    public static object GetPropertyValue(this object obj, string propName)
    {

        var type = obj.GetType();

        var accessor = TypeAccessor.Create(type);

       return  accessor[obj, propName];

        //not required until above code throws an exception

        //var valueToReturn = obj.GetType().GetProperty(propName, System.Reflection.BindingFlags.Public
        //                                                        |System.Reflection.BindingFlags.NonPublic
        //                                                        |System.Reflection.BindingFlags.Instance)?.GetValue(obj);

        //return valueToReturn;

    }

    public static TBindingType GetPropertyValue<TModel, TBindingType>(this TModel obj, string propName) where TBindingType : notnull
    {
        var accessor = TypeAccessor.Create(typeof(TModel));

        var valueToReturn = accessor[obj, propName];

        //not required until above code throws an exception
        //var valueToReturn = obj.GetType().GetProperty(propName)?.GetValue(obj);

        if (valueToReturn is null) return default;

        return (TBindingType)valueToReturn;
        //var result = TryParse<TBindingType>(valueToReturn, out TBindingType typedValue);

        //return valueToReturn;

    }


    //public static object GetDefaultValue(this Type objectType)
    //{
    //    if (objectType.IsValueType && Nullable.GetUnderlyingType(objectType) == null)
    //    {
    //        return Activator.CreateInstance(objectType);
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}

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

    //public static T[] AddParam<T>(this T[] parameters, T newParameter)
    //{
    //    var result = new T[parameters.Length + 1];

    //    parameters[0] = newParameter;

    //    parameters.CopyTo(result, 1);



    //    return result;
    //}

    internal static IEnumerable<TType> Exclude<TType, ExcludeType>(this IEnumerable<TType> list)
    {
        return list.Where(x => x is not ExcludeType).Cast<TType>();
    }


    //public static bool TryParse<TBindingField>(this object targetText, out TBindingField returnValue)
    //{
    //    bool returnStatus = false;

    //    returnValue = default(TBindingField);

    //    try
    //    {
    //        var type = typeof(TBindingField);
    //        var converter = TypeDescriptor.GetConverter(type);


    //        if (converter != null && converter.IsValid(targetText))
    //        {
    //            returnValue = (TBindingField)converter.ConvertFrom(targetText);
    //            returnStatus = true;
    //        }
    //        else
    //        {
    //            returnValue = (TBindingField)Convert.ChangeType(targetText, typeof(TBindingField));
    //            returnStatus = true;
    //        }

    //    }
    //    catch
    //    {
    //        // just swallow the exception and return the default values for failure
    //    }

    //    return (returnStatus);

    //}

    //public static bool TryParse(this object targetText,Type objectType, out object returnValue)
    //{
    //    bool returnStatus = false;

    //    returnValue = objectType.GetDefaultValue();

    //    try
    //    {
    //        var type = objectType;
    //        var converter = TypeDescriptor.GetConverter(type);


    //        if (converter != null && converter.IsValid(targetText))
    //        {
    //            returnValue = converter.ConvertFrom(targetText);
    //            returnStatus = true;
    //        }
    //        else
    //        {
    //            returnValue = Convert.ChangeType(targetText, objectType);
    //            returnStatus = true;
    //        }

    //    }
    //    catch
    //    {
    //        // just swallow the exception and return the default values for failure
    //    }

    //    return (returnStatus);

    //}
}
