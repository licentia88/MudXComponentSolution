using System;
using MudXComponents.Enums;

namespace MudXComponents.Args;

public class ProcessEventArgs<T>
{
    public T RequestData { get; set; }

    public T ResponseData { get; set; }

    public dynamic DynamicArgs { get; set; }

    public ViewState ViewState { get; set; }
 
}

