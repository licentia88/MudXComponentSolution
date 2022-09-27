using Microsoft.AspNetCore.Components;
using MudXComponents.Enums;
using System.ComponentModel;

namespace MudXComponents.Components
{
    public class GridSpacer<TModel> : ColumnBase<TModel> where TModel : new()
    {

        [Parameter, EditorBrowsable(EditorBrowsableState.Never)]
        public ComponentTypes ComponentType { get; set; }

        protected override Task OnInitializedAsync()
        {
            Console.WriteLine($"am I here Yet");

            if (ParentComponent is null)
            {
                Console.WriteLine("parent is null");
            }
            return base.OnInitializedAsync();
        }
    }
}

