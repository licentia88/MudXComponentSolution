using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using MudXComponents.Components;
using MudXComponents.Enums;

namespace MudXComponents.Extensions;
public static class RenderExtensions
{

    //public static void RenderUIComponent<TModel>(this ColumnBase<TModel> component, Type GeneruicComponent, RenderTreeBuilder builder, params (string key, object value)[] parameters) where TModel : new()
    //{

    //    builder.OpenComponent(0, GeneruicComponent);

    //    var properties = component.GetType().GetProperties().Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(ParameterAttribute)));

    //    foreach (var property in properties.Select((val, index) => (val, index)))
    //    {
    //        var index = property.index + 1;

    //        var propName = property.val.Name;

    //        var value = component.GetPropertyValue(propName);

    //        builder.AddAttribute(index, propName, value);


    //    }

    //    builder.AddAttribute(99, nameof(component.Context), component.Context);

    //    foreach (var additionalParameters in parameters)
    //    {
    //        builder.AddAttribute(101, additionalParameters.key, additionalParameters.value);
    //    }



    //    builder.CloseComponent();

    //}

    public static void Render<TModel>(this ColumnBase<TModel> component, RenderTreeBuilder builder, TModel context, ComponentTypes componentType, params (string key, object value)[] parameters) where TModel : new()
    {
        var properties = component.GetType().GetProperties().Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(ParameterAttribute)));

        builder.OpenComponent(0, component.GetType());

        foreach (var property in properties.Select((val, index) => (val, index)))
        {
            var index = property.index + 1;

            var propName = property.val.Name;

            var value = component.GetPropertyValue(propName);

            builder.AddAttribute(index, propName, value);

        }

        builder.AddAttribute(99, nameof(component.Context), context);


        if (component.HasProperty("ComponentType"))
        {
            //Console.WriteLine($"Adding attribute {componentType}");
            builder.AddAttribute(100, nameof(componentType), componentType);
        }


        foreach (var additionalParameters in parameters)
        {
            builder.AddAttribute(101, additionalParameters.key, additionalParameters.value);
        }



        builder.CloseComponent();
    }


}
