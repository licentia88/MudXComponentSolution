using System;
using MudXComponents.Components;
using MudXComponents.Enums;

namespace MudXComponents.Args;

public class GridXArgs<TModel> where TModel: new()
{
    public MudXPage<TModel> Page { get; set; }

    public TModel OldData { get; set; }

    public TModel NewData { get; set; }

    public int Index { get; set; }
}

