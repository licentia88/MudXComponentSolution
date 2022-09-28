﻿using System;
using Microsoft.AspNetCore.Components;
using MudXComponents.Enums;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;
using MudXComponents.Extensions;

namespace MudXComponents.Components;


public class ColumnBase<TModel> : ComponentBase where TModel : new()
{
    [CascadingParameter(Name = nameof(ParentComponent))]
    public MudGridX<TModel> ParentComponent { get; set; }

    [Parameter]
    public string Class { get; set; }

    [Parameter]
    public string Style { get; set; }

    [Parameter]
    public string BindingField { get; set; }

    [Parameter, AllowNull]
    public int Order { get; set; }

    [Parameter, AllowNull]
    [Range(1, 12, ErrorMessage = "Column width must be between 1 and 12")]
    public int Width { get; set; }

    [Parameter, AllowNull]
    public bool VisibleOnEdit { get; set; } = true;

    [Parameter, AllowNull]
    public bool EnabledOnEdit { get; set; } = true;


    [Parameter]
    public string Label { get; set; }

    #region Sizes

    [Parameter]
    public int xs { get; set; }

    [Parameter]
    public int sm { get; set; }

    [Parameter]
    public int md { get; set; }

    [Parameter]
    public int lg { get; set; }

    [Parameter]
    public int xl { get; set; }

    [Parameter]
    public int xxl { get; set; }

    #endregion

    [Parameter]
    public virtual TModel Context { get; set; }




    public virtual RenderFragment RenderComponent(TModel context, ComponentTypes componentType) => builder =>
    {
        Context = context;
        this.Render(builder, context, componentType);
    };

    protected override void OnInitialized()
    {

        ParentComponent?.AddChildComponent(this);
    }
}
