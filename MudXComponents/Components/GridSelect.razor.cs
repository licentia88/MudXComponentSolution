using System;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudXComponents.Enums;
using MudXComponents.Extensions;
using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Web;

namespace MudXComponents.Components;

public partial class GridSelect<TModel, TSourceModel> : ColumnBase<TModel> where TModel : new() where TSourceModel : class, new ()
{
    public TSourceModel CurrentValue { get; set; }

    [Parameter]
    public EventCallback<TSourceModel> ValueChanged { get; set; }

    [Parameter]
    public string Placeholder { get; set; }

    [Parameter]
    public string PopoverClass { get; set; }

    [Parameter]
    public string UncheckedIcon { get; set; }

    [Parameter]
    public Origin TransformOrigin { get; set; } = Origin.TopCenter;

    [Parameter,EditorRequired]
    public string TextField { get; set; }

    [Parameter, EditorRequired]
    public string ValueField { get; set; }


    [Parameter, EditorBrowsable(EditorBrowsableState.Never)]
    public Func<TSourceModel,bool> WhereStatement { get; set; }


    [Parameter, EditorRequired]
    public IEnumerable<TSourceModel> DataSource { get; set; }

    [Parameter, EditorBrowsable(EditorBrowsableState.Never)]
    public ComponentTypes ComponentType { get; set; }

    protected virtual Func<TSourceModel, string> StringConverter => x => x?.GetPropertyValue(TextField)?.ToString();

    [Parameter]
    public string Delimiter { get; set; }

    [Parameter]
    public bool DisableUnderLine { get; set; }

    [Parameter]
    public bool Error { get; set; }

    [Parameter]
    public string Format { get; set; }

    [Parameter]
    public bool FullWidth { get; set; }

    [Parameter]
    public string HelperText { get; set; }

    [Parameter]
    public bool HelperTextOnFocus { get; set; }

    [Parameter]
    public InputMode InputMode { get; set; }

    [Parameter]
    public string IndeterminateIcon { get; set; }

    [Parameter]
    public Size IconSize { get; set; }

    [Parameter]
    public int Lines { get; set; }

    [Parameter]
    public Margin Margin { get; set; }

    [Parameter]
    public IMask Mask { get; set; }

    [Parameter]
    public Dictionary<string, object> UserAttributes { get; set; }

    [Parameter]
    public bool LockScroll { get; set; }

    [Parameter]
    public bool TextUpdateSuppression { get; set; }

    [Parameter]
    public bool KeyDownPreventDefault { get; set; }

    [Parameter]
    public bool KeyPressPreventDefault { get; set; }

    [Parameter]
    public bool KeyUpPreventDefault { get; set; }

    [Parameter]
    public int MaxLength { get; set; } = 524288;

    [Parameter]
    public string Pattern { get; set; }

    [Parameter]
    public object Tag { get; set; }

    [Parameter]
    public string ErrorId { get; set; }

    [Parameter]
    public string ErrorText { get; set; }

    [Parameter]
    public string RequiredError { get; set; }

    [Parameter]
    public Expression<Func<TSourceModel>> For { get; set; }

    [Parameter]
    public CultureInfo Culture { get; set; }

    [Parameter]
    public bool Dense { get; set; }

    [Parameter]
    public Direction Direction { get; set; }

    [Parameter]
    public bool Immediate { get; set; }

    [Parameter]
    public Origin AnchorOrigin { get; set; } = Origin.BottomCenter;

    [Parameter]
    public string CheckedIcon { get; set; }

    [Parameter]
    public string CloseIcon { get; set; }

    [Parameter]
    public int? Counter { get; set; }

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
    public bool ReadOnly { get; set; }

    [Parameter]
    public bool Required { get; set; }

   
  

    public void OnValueChanged(TSourceModel model)
    {
        Context.SetPropertyValue(BindingField, model.GetPropertyValue(ValueField));
    }
}

