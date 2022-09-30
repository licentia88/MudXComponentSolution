﻿using System;
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
        public bool EnableModelValidation { get; set; }

        [Parameter]
        public ViewState ViewState { get; set; }

        [Parameter]
        public string SubmitText { get; set; } = string.Empty;

        [Parameter]
        public string CancelText { get; set; } = string.Empty;

        [Parameter]
        public string DialogTitle { get; set; } = string.Empty;

        [Parameter]
        public bool IsChild { get; set; }

        [Parameter]
        public bool SmartCrud { get; set; }
        
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

        [Parameter]
        public EventCallback<MudXPage<TModel>> OnLoad { get; set; }
        
        protected override async Task OnInitializedAsync()
        {
            SubmitText = SubmitText.Equals(string.Empty)? ViewState.ToString() :SubmitText;

            CancelText = CancelText.Equals(string.Empty) ? "Cancel" : CancelText;

            await base.OnInitializedAsync();

            await OnLoad.InvokeAsync(this);
        }

        protected virtual void Cancel()
        {
            MudDialog.Cancel();
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

