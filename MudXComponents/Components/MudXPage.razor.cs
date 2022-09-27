using System;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MudXComponents.Enums;
using MudXComponents.Extensions;

namespace MudXComponents.Components
{
	public partial class MudXPage<TModel> : UIMudBase<TModel> where TModel : new() 
	{
        [Parameter]
        public ViewState ViewState { get; set; }

        [Parameter]
        public string SubmitText { get; set; } = string.Empty;

        [Parameter]
        public string CancelText { get; set; } = string.Empty;

        [Parameter]
        public string DialogTitle { get; set; } = string.Empty;

        protected virtual void Cancel() => MudDialog.Cancel();

        [Parameter, EditorBrowsable(EditorBrowsableState.Never)]
        public RenderFragment DetailGrid { get; set; }

        [Parameter]
        public ObservableCollection<ColumnBase<TModel>> Components { get; set; }

        [Parameter]
        public EventCallback<TModel> OnCreate { get; set; }

        [Parameter]
        public EventCallback<TModel> OnUpdate { get; set; }

        [Parameter]
        public EventCallback<TModel> OnDelete { get; set; }


        protected override Task OnInitializedAsync()
        {
            Console.WriteLine($"Model Username : {ViewModel.GetPropertyValue("UserName")}");

            SubmitText = ViewState.ToString();
            return base.OnInitializedAsync();
        }


        protected async ValueTask SubmitClick()
        {
            if (ViewModel is null) return;

            if (ViewState == ViewState.Create)
            {
                await OnCreate.InvokeAsync(ViewModel);
            }
            else if (ViewState == ViewState.Update)
            {
                await OnUpdate.InvokeAsync(ViewModel);
            }
            else if (ViewState == ViewState.Remove)
            {
                await OnDelete.InvokeAsync(ViewModel);
            }
            else
            {
                return;
            }

            MudDialog.Close();
        }
    }
}

