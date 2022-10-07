using System;
using MudXComponents.Enums;

namespace MudXComponents.Args;

public class GridXArgs<TModel>
{
    public TModel OldData { get; set; }

    public TModel NewData { get; set; }

    public int Index { get; set; }
}

