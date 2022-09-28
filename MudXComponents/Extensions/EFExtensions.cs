using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MudXComponents.Extensions;

public static class EFExtensions
{
    /// <summary>
    /// Gets primary key value of object if property is decoreted with KeyAttribute 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public static object GetPrimaryKeyValue<TModel>(this TModel model) //where TModel : class
    {
        if (model is null) throw new NullReferenceException($"{typeof(TModel).Name} is null");

        var key = GetKey(typeof(TModel));

        return model!.GetType().GetProperty(key)?.GetValue(model);
    }

    /// <summary>
    /// Gets Primary Key name of given type if property is decorated with KeyAttribute
    /// </summary>
    /// <param name="entityType"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public static string GetKey(this Type entityType)
    {
        var test = entityType.GetProperties().ToList();
        var attributeInfo = entityType.GetProperties()
            .FirstOrDefault(x => x.CustomAttributes.Any(y => y.AttributeType.Equals(typeof(KeyAttribute))));

        if (attributeInfo is null) throw new KeyNotFoundException("Key not found");

        return attributeInfo.Name;
    }

    /// <summary>
    /// Returns foreign key name of model if property is decorated with ForeignKey attribute
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="childName"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public static string GetForeignKey(this Type parent, string childName)
    {
        var property = parent.GetProperty(childName);

        if (property is null) throw new KeyNotFoundException("Child Navigation property not implemented");

        var attributeInfo = property.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(ForeignKeyAttribute));

        if (attributeInfo is null) throw new KeyNotFoundException("Key not found");

        var foreignKey = attributeInfo.ConstructorArguments.First().ToString();

        return foreignKey;
    }

    /// <summary>
    /// Returns foreign key name of model if property is decorated with ForeignKey attribute
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="childName"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public static string GetForeignKey<TParent, TChild>() //where TParent : class where TChild : class
    {
        var property = typeof(TParent).GetProperties().
        FirstOrDefault(x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericArguments()[0].IsAssignableFrom(typeof(TChild)));

        //var property = typeof(TParent).GetProperty(typeof(TChild).Name);

        if (property is null) throw new KeyNotFoundException("Child Navigation property not implemented");

        var attributeInfo = property.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(ForeignKeyAttribute));

        if (attributeInfo is null) throw new KeyNotFoundException("Key not found");

        var foreignKey = attributeInfo.ConstructorArguments.First();

        return foreignKey.Value.ToString();
    }
}
