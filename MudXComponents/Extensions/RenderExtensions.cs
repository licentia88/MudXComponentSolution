﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using MudBlazor;
using MudXComponents.Components;
using MudXComponents.Enums;

namespace MudXComponents.Extensions;
public static class RenderExtensions
{

    public static void Render<TModel>(this ColumnBase<TModel> component, RenderTreeBuilder builder, TModel context, ComponentTypes componentType, params (string key, object value)[] parameters) where TModel : new()
    {
        var properties = component.GetType().GetProperties().Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(ParameterAttribute)));

        builder.OpenComponent(0, component.GetType());

        foreach (var property in properties.Select((val, index) => (val, index)))
        {
            var index = property.index + 1;

            var propName = property.val.Name;

            var value = component.GetPropertyValue(propName);

            //Console.WriteLine(value);
            builder.AddAttribute(index, propName, value);

        }

        builder.AddAttribute(99, nameof(component.Context), context);

        if (component.HasProperty(nameof(GridColumn<TModel, Default>.ComponentType)))
        {
            Console.WriteLine($"Adding attribute {componentType}");
            builder.AddAttribute(100, nameof(componentType), componentType);
        }


        foreach (var additionalParameters in parameters)
        {
            builder.AddAttribute(101, additionalParameters.key, additionalParameters.value);
        }



        builder.CloseComponent();
    }


}