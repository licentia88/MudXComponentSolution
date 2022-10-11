using System;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MudXComponents.Enums;
using MudXComponents.Extensions;
using Microsoft.Extensions.Caching.Memory;
using static MudBlazor.CategoryTypes;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components.Forms;
using MudXComponents.Args;

namespace MudXComponents.Components
{
	public partial class MudXPage<TModel> : UIMudBase<TModel>, IDisposable where TModel : new() 
	{
        private GridXArgs<TModel> CreateArgs => new GridXArgs<TModel> { Index = Index, OldData = Original, NewData = ViewModel };


        [Inject]
        public IMemoryCache MemoryCache { get; set; }

        [Parameter]
        public TModel Original { get; set; }

        [Parameter]
        public int Index { get; set; }

        [Parameter,EditorBrowsable(EditorBrowsableState.Never)]
        public MudGridX<TModel> ParentGrid { get; set; }

        [Parameter]
        public object ParentContext { get; set; }

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
        public bool SmartCrud { get;  set; }
        
        [Parameter, EditorBrowsable(EditorBrowsableState.Never)]
        public RenderFragment DetailGrid { get; set; }

        [Parameter]
        public ObservableCollection<ColumnBase<TModel>> Components { get; set; }

        [Parameter]
        public EventCallback<GridXArgs<TModel>> OnCreate { get; set; }

        [Parameter]
        public EventCallback<GridXArgs<TModel>> OnUpdate { get; set; }

        [Parameter]
        public EventCallback<GridXArgs<TModel>> OnDelete { get; set; }

        [Parameter]
        public EventCallback<GridXArgs<TModel>> OnBeforeSubmit{ get; set; }

        [Parameter]
        public EventCallback<MudXPage<TModel>> OnLoad { get; set; }

        private string CacheKey { get; set; }
      
        protected override async Task OnInitializedAsync()
        {

            ViewModel = ViewModel.ToImmutable();

            CacheKey = MemoryCache.Get<String>(nameof(CacheKey));

            SubmitText = SubmitText.Equals(string.Empty)? ViewState.ToString() :SubmitText;

            CancelText = CancelText.Equals(string.Empty) ? "Cancel" : CancelText;


            await OnLoad.InvokeAsync(this);

            await base.OnInitializedAsync();
        }

        public async void OnExpire()
        {
            await InvokeAsync(() => MudDialog.CancelAll());
        }

        protected virtual void Cancel()
        {
            ViewModel = Original;
            MudDialog.Cancel();
        }

        protected async ValueTask SubmitClick()
        {

            if (ViewModel is null) return;

              
            await  OnBeforeSubmit.InvokeAsync(CreateArgs);

            if (ViewState == ViewState.Create)
            {
                if (IsChild && SmartCrud)
                {
                    var listToAdd = ParentContext.GetPropertyValue(typeof(TModel).Name) as ICollection<TModel>;

                    listToAdd.Add(ViewModel);
                    //MemoryCache.Set()
                    //var method = listToAdd.GetType().GetMethod("Add");

                    ParentGrid.DataSource.Add(ViewModel);

                    MemoryCache.Set($"{CacheKey}{typeof(TModel).Name}", ParentGrid.DataSource);
                    //method.Invoke(listToAdd, new[] { (object)ViewModel });
                }
                else
                {
                    await OnCreate.InvokeAsync(CreateArgs);
                }
            }
            else if (ViewState == ViewState.Update)
            {
                if (IsChild && SmartCrud)
                {
                    var listToAdd = ParentContext.GetPropertyValue(typeof(TModel).Name) as IList<TModel>;

                    var index = listToAdd.IndexOf(Original);
 
                    listToAdd[index] = ViewModel;

                    ParentGrid.DataSource = listToAdd.ToObservableCollection();

                    MemoryCache.Set($"{CacheKey}{typeof(TModel).Name}", ParentGrid.DataSource);
                    //method.Invoke(listToAdd, new[] { (object)ViewModel });
                }
                else
                {
                    await OnUpdate.InvokeAsync(CreateArgs);
                }

                await OnUpdate.InvokeAsync(CreateArgs);
            }
            else if (ViewState == ViewState.Remove)
            {
                await OnDelete.InvokeAsync(CreateArgs);
            }
            else
            {
                return;
            }

            MudDialog.Close();
        }

        /// <summary>
        /// Gets component by bindingfield
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public TComponent GetComponent<TComponent>(string FieldName) where TComponent :ColumnBase<TModel>
        {
            var component = Components.FirstOrDefault(x => x.BindingField is not null && x.BindingField == FieldName);

            return (TComponent)component;
        }

        public new void StateHasChanged()
        {
            base.StateHasChanged();
        }
        public void Dispose()
        {
            //TODO: MEMORY CACHE DISPOSE OLDUGUNDA ICINDEKI DATALARIN SILINDIGINDEN EMIN OL
        }
    }
}

