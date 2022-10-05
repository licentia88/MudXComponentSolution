using System;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudXComponents.Enums;
using MudXComponents.Extensions;
using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;

namespace MudXComponents.Components;

public partial class GridCheckBox<TModel> : ColumnBase<TModel> where TModel : new() 
{
    [Parameter]
    public string TrueText { get; set; }

    [Parameter]
    public string FalseText { get; set; }


    [Parameter]
    public bool ReadOnly { get; set; }

    [Parameter]
    public string IndeterminateIcon { get; set; }

    [Parameter]
    public Color Color { get; set; }
    
    [Parameter]
    public string  CheckedIcon { get; set; }

    [Parameter]
    public bool Error { get; set; }

    [Parameter]
    public LabelPosition LabelPosition { get; set; }

    [Parameter]
    public string ErrorId { get; set; }

    [Parameter]
    public string ErrorText { get; set; }

    [Parameter]
    public string RequiredError { get; set; }

    [Parameter, EditorBrowsable(EditorBrowsableState.Never)]
    public ComponentTypes ComponentType { get; set; }

    [Parameter]
    public CultureInfo Culture { get; set; }

    [Parameter]
    public bool Dense { get; set; }


    [Parameter]
    public Variant Variant { get; set; } = Variant.Outlined;

    [Parameter]
    public int MaxHeight { get; set; } = 300;

    [Parameter]
    public bool MultiSelection { get; set; }

    [Parameter]
    public Func<List<string>, string> MultiSelectionTextFunc { get; set; }

    [Parameter]
    public bool SelectAll { get; set; }

    [Parameter]
    public string SelectAllText { get; set; }

    [Parameter]
    public Adornment Adornment { get; set; } = Adornment.End;

    [Parameter]
    public string AdornmentAriaLabel { get; set; }

    [Parameter]
    public string OpenIcon { get; set; }

    [Parameter]
    public Color AdornmentColor { get; set; }

    [Parameter]
    public string AdornmentIcon { get; set; } = Icons.Filled.ArrowDropDown;

    [Parameter]
    public string AdornmentText { get; set; }

    [Parameter]
    public bool AutoFocus { get; set; }

    [Parameter]
    public bool Clearable { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public bool KeyboardEnabled { get; set; }

    [Parameter]
    public bool Required { get; set; }

    [Parameter]
    public Size Size { get; set; }

    [Parameter]
    public bool StopClickPropagation { get; set; }

    [Parameter]
    public Color UnCheckedColor { get; set; }

    [Parameter]
    public string UncheckedIcon { get; set; }

    [Parameter]
    public Func<bool, IEnumerable<string>> Validation { get; set; }



    private void OnValueChanged(bool value)
    {
        Context.SetPropertyValue(BindingField, value);
    }
}

