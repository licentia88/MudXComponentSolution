using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudXComponents.Interfaces;
using System.Diagnostics.CodeAnalysis;
using MudXComponents.Enums;

namespace MudXComponents.Components;

public partial class GridFabButton<TModel> : ColumnBase<TModel>, IButton where TModel : new()
{
    public string BindingField { get; set; }

    [Parameter, AllowNull]
    public EventCallback<TModel> OnClick { get; set; }

    [Parameter, AllowNull]
    public MaxWidth PageSize { get; set; }

    [Parameter, AllowNull]
    public Color Color { get; set; }

    [Parameter, AllowNull]
    public Size Size { get; set; }

    [Parameter, AllowNull]
    public string StartIcon { get; set; }

    [Parameter, AllowNull]
    public bool Disabled { get; set; }

    [Parameter, AllowNull]
    public string Title { get; set; }

    [Parameter, AllowNull]
    public string Tag { get; set; }

    [Parameter, AllowNull]
    public string Href { get; set; }

    [Parameter, AllowNull]
    public ButtonType ButtonType { get; set; }

    [Parameter, AllowNull]
    public string EndIcon { get; set; }

    [Parameter, AllowNull]
    public string Target { get; set; }


    [Parameter, AllowNull]
    public Color IconColor { get; set; } = Color.Inherit;


    [Parameter, AllowNull]
    public Size IconSize { get; set; }

    [Parameter, AllowNull]
    public bool DisableElevation { get; set; }

    [Parameter, AllowNull]
    public bool DisableRipple { get; set; }

    [Parameter, AllowNull]
    public ViewState ViewState { get; set; } = ViewState.None;


    [CascadingParameter(Name = nameof(Context))]
    public override TModel Context { get; set; }

    protected override void OnInitialized()
    {
        Size = Size.Small;
        Title = Title ?? ParentComponent?.Title;
        base.OnInitialized();
    }

}
