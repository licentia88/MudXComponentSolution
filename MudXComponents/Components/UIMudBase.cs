using System;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using MudBlazor;

namespace MudXComponents.Components;

public class UIMudBase<TModel> : UIBase where TModel : new () 
{
  
    [CascadingParameter]
    public MudDialogInstance MudDialog
    {
        get; set;
    }
}
