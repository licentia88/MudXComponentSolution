using Microsoft.AspNetCore.Components;
using MudXComponents.Enums;
using MudBlazor;
using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;

namespace MudXComponents.Components;

public partial class GridColumn<TModel, BindingType> : ColumnBase<TModel> where TModel : new()
{

    [Parameter, EditorBrowsable(EditorBrowsableState.Never)]
    public ComponentTypes ComponentType { get; set; }

    [Parameter]
    public bool Immediate { get; set; }

    [Parameter]
    public bool ReadOnly { get; set; }

    [Parameter]
    public bool Required { get; set; }

    [Parameter]
    public Variant Variant { get; set; }

    [Parameter]
    public Adornment Adornment { get; set; }

    [Parameter]
    public string AdornmentAriaLabel { get; set; }

    [Parameter]
    public Color AdornmentColor { get; set; }

    [Parameter]
    public string AdornmentIcon { get; set; }

    [Parameter]
    public string AdornmentText { get; set; }

    [Parameter]
    public bool AutoFocus { get; set; }

    [Parameter]
    public bool Clearable { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public bool DisableUnderLine { get; set; }

    [Parameter]
    public int? Counter { get; set; }


    [Parameter]
    public int DebounceInterval { get; set; }

    //[Parameter]
    //public MudBlazor.Converter<BindingType, string> Converter { get; set; }

    [Parameter]
    public CultureInfo Culture { get; set; }

    [Parameter]
    public bool Error { get; set; }


    [Parameter]
    public string ErrorId { get; set; }

    [Parameter]
    public string ErrorText { get; set; }

    [Parameter]
    public string RequiredError { get; set; }

    [Parameter]
    public Expression<Func<BindingType>> For { get; set; }

    [Parameter]
    public string Format { get; set; }

    [Parameter]
    public bool FullWidth { get; set; }

    [Parameter]
    public string HelperText { get; set; }

    [Parameter]
    public bool HelperTextOnFocus { get; set; }


    [Parameter]
    public Size IconSize { get; set; }

    [Parameter]
    public InputMode InputMode { get; set; }


    [Parameter]
    public InputType InputType { get; set; }


    [Parameter]
    public int Lines { get; set; }

    [Parameter]
    public Margin Margin { get; set; }

    [Parameter]
    public IMask Mask { get; set; }


    [Parameter]
    public int MaxLength { get; set; } = 524288;

    [Parameter]
    public string Pattern { get; set; }

    [Parameter]
    public string Placeholder { get; set; }

    [Parameter]
    public object Tag { get; set; }


    [Parameter]
    public Func<BindingType, bool> Validation { get; set; }


    [Parameter]
    public Dictionary<string, object> UserAttributes { get; set; }



    [Parameter]
    public bool TextUpdateSuppression { get; set; }

    [Parameter]
    public bool KeyDownPreventDefault { get; set; }

    [Parameter]
    public bool KeyPressPreventDefault { get; set; }

    [Parameter]
    public bool KeyUpPreventDefault { get; set; }

}

