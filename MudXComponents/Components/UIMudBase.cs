using System;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using MudBlazor;

namespace MudXComponents.Components;

public class UIMudBase<TModel> : UIBase
{
    [Parameter, AllowNull]
    public TModel ViewModel { get; set; }

    [CascadingParameter]
    public MudDialogInstance MudDialog
    {
        get; set;
    }
}
