using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudXComponents.Enums;
using MudXComponents.Extensions;
using MudXComponents.Interfaces;

namespace MudXComponents.Components
{
     

    public partial class MudGridX<TModel> : UIMudBase<TModel> where TModel : new()
    {
     

        [CascadingParameter(Name =nameof(SmartCrud))]
        private bool _smartCrud { get; set; }

        [Parameter, AllowNull]
        public bool SmartCrud { get { return _smartCrud; } set { _smartCrud = value; } }

 

        [Inject]
        public IMemoryCache MemoryCache { get; set; }



        [CascadingParameter(Name = nameof(ParentContext))]
        public object ParentContext { get; set; }

        [Parameter,AllowNull]
        public bool EnableModelValidation { get; set; } 
        //private MudTable<TModel> MyTable { get; set; }


        [Parameter, AllowNull]
        public ObservableCollection<TModel> DataSource { get; set; }

        /// <summary>
        /// Displays the Search bar
        /// </summary>
        [Parameter]
        public bool EnableSearch { get; set; }

        /// <summary>
        /// Displays Pagination
        /// </summary>
        [Parameter]
        public bool EnablePagination { get; set; }

        #region Page Information

        [Parameter, AllowNull]
        public string Title { get; set; }

        #endregion Page Commands

        [Parameter, AllowNull]
        public virtual EventCallback<TModel> OnCreate { get; set; }

        [Parameter, AllowNull]
        public virtual EventCallback<TModel> OnDelete { get; set; }

        [Parameter, AllowNull]
        public virtual EventCallback<TModel> OnUpdate { get; set; }

        [Parameter, AllowNull]
        public virtual EventCallback<TModel> OnBeforeSubmit { get; set; }


        [Parameter, AllowNull]
        public virtual EventCallback<MudXPage<TModel>> OnLoad { get; set; }



        #region Dialog

        [Inject]
        protected IDialogService DialogService { get; set; }

        protected DialogResult DialogResult { get; set; }

        private DialogParameters Parameters { get; set; } = new DialogParameters();

        protected  DialogOptions Options { get; set; }

        

        #endregion Dialog

        #region RenderFragments

        [Parameter, AllowNull]
        public RenderFragment GridColumns { get; set; }

        [Parameter, AllowNull]
        public RenderFragment HeaderButtons { get; set; }

        [Parameter, AllowNull]
        public RenderFragment<TModel> GridButtons { get; set; }

        [Parameter, AllowNull]
        public RenderFragment DetailGrid { get; set; }

        private string _searchString = "";

        public ObservableCollection<ColumnBase<TModel>> Components { get; set; } = new();

        private bool isFirstRender = true;

        [CascadingParameter(Name =nameof(IsChild))]
        internal bool IsChild { get; set; }

      

        private MemoryCacheEntryOptions options(IMemoryCache cache) => new MemoryCacheEntryOptions()
                               .SetSlidingExpiration(TimeSpan.FromMinutes(10))
                               .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                               //.SetPriority(CacheItemPriority.NeverRemove)
                               .RegisterPostEvictionCallback(PostEvictionCallBack, cache);

        public string CacheKey { get; set; }

        public MudGridX()
        {
            Options = new DialogOptions
            {
                MaxWidth = MaxWidth.Medium,
                FullWidth = true,
                CloseButton = true,
                CloseOnEscapeKey = true,
                DisableBackdropClick = true,
                Position = DialogPosition.Center
            };


           
        }


        protected override Task OnInitializedAsync()
        {
            if (!IsChild)
            {
                CacheKey = Guid.NewGuid().ToString();

                MemoryCache.Set<string>(nameof(CacheKey), CacheKey, options(MemoryCache));

                CacheKey = MemoryCache.Get<string>(nameof(CacheKey));
            }

            if(IsChild)
            {
                CacheKey = MemoryCache.Get<string>(nameof(CacheKey));

                ReloadDataSource();
            }




            return base.OnInitializedAsync();
        }


        private List<TType> GetComponentOf<TType>()
        {
            return  Components.
                Where(x => x is TType).
                Exclude<ColumnBase<TModel>, IButton>()
                .Exclude<ColumnBase<TModel>, GridSpacer<TModel>>()
                .Cast<TType>().ToList();
        }


        /// Add a child component (will be done by the child itself)
        public void AddChildComponent(ColumnBase<TModel> pChildComponent)
        {

            if (GetComponentOf<ColumnBase<TModel>>().Any(x => x.BindingField == pChildComponent.BindingField))
            {
                return;
            }

            if (pChildComponent is GridFabButton<TModel> button)
            {
                if (!button.OnClick.HasDelegate && button.ViewState != ViewState.None)
                {

                    var CurrentModel = button.Context ?? new();

                    var paramList = new List<(string key, object value)>();

                    paramList.Add((nameof(MudXPage<TModel>.Components), Components));
                    paramList.Add((nameof(MudXPage<TModel>.OnCreate), OnCreate));
                    paramList.Add((nameof(MudXPage<TModel>.OnUpdate), OnUpdate));
                    paramList.Add((nameof(MudXPage<TModel>.OnDelete), OnDelete));
                    paramList.Add((nameof(MudXPage<TModel>.OnLoad), OnLoad));
                    paramList.Add((nameof(MudXPage<TModel>.OnBeforeSubmit), OnBeforeSubmit));
 
                    paramList.Add((nameof(MudXPage<TModel>.EnableModelValidation), EnableModelValidation));
                    paramList.Add((nameof(MudXPage<TModel>.DetailGrid), DetailGrid));
                    paramList.Add((nameof(MudXPage<TModel>.ViewState), button.ViewState));
                    paramList.Add((nameof(MudXPage<TModel>.DialogTitle), button.Title));
                    paramList.Add((nameof(MudXPage<TModel>.IsChild), IsChild));
                    paramList.Add((nameof(MudXPage<TModel>.SmartCrud), SmartCrud));
                    paramList.Add((nameof(MudXPage<TModel>.ParentContext), ParentContext));
                    paramList.Add((nameof(MudXPage<TModel>.ParentGrid), this));
                    //ParentGrid
                    button.OnClick = EventCallback.Factory.Create<TModel>(this, callback: async () => await ShowDialogAsync<MudXPage<TModel>>(CurrentModel,button.PageSize, paramList.ToArray()));

                    
                }
            }

             Components.Add(pChildComponent);

        }

 
        #endregion RenderFragments

       

        protected virtual TModel OnClose()
        {
            return !DialogResult.Cancelled ? ViewModel : default;
        }

        public virtual async ValueTask<DialogResult> ShowDialogAsync<TMudXPage>(params (string key, object value)[] parameters) where TMudXPage : UIBase
        {
          

            foreach (var prm in parameters)
            {
                Parameters.Add(prm.key, prm.value);
            }


            var dialogTItle = parameters.FirstOrDefault(x => x.key.Equals(nameof(MudXPage<TModel>.DialogTitle)));

            var dialogReference = DialogService.Show<TMudXPage>(dialogTItle.value?.ToString(), Parameters, Options);

            return await dialogReference.Result;

            

         }


        public virtual async ValueTask<DialogResult> ShowDialogAsync<TMudXPage>(TModel viewModel ,params (string key, object value)[] parameters) where TMudXPage : UIBase
        {
            var stringifiedModel = JsonSerializer.Serialize(viewModel);

            var cloned = JsonSerializer.Deserialize<TModel>(stringifiedModel);

            var newParamList = parameters.ToList();

            newParamList.Add((nameof(ViewModel), cloned));

            return await ShowDialogAsync<TMudXPage>(newParamList.ToArray());
        }

        public virtual async ValueTask<DialogResult> ShowDialogAsync<TMudXPage>(MaxWidth pageSize, params (string key, object value)[] parameters) where TMudXPage : UIBase
        {
            Options.MaxWidth = pageSize;

            return await ShowDialogAsync<TMudXPage>(parameters);
        }

        public virtual async ValueTask<DialogResult> ShowDialogAsync<TMudXPage>(TModel viewModel, MaxWidth pageSize, params (string key, object value)[] parameters) where TMudXPage : UIBase
        {
            var stringifiedModel = JsonSerializer.Serialize(viewModel);

            var cloned = JsonSerializer.Deserialize<TModel>(stringifiedModel);

            var newParamList = parameters.ToList();

            newParamList.Add((nameof(MudXPage<TModel>.ViewModel), cloned));
            newParamList.Add((nameof(MudXPage<TModel>.Original), viewModel));

            Options.MaxWidth = pageSize;

            var dialogResult = await ShowDialogAsync<TMudXPage>(newParamList.ToArray());


        

            return dialogResult;

        }

        private void ReloadDataSource()
        {
            var existingData = MemoryCache.Get<ObservableCollection<TModel>>($"{CacheKey}{typeof(TModel).Name}");

            if (existingData is not null && existingData.Any())
            {
                foreach (var data in existingData)
                {
                    DataSource.Add(data);
                }
            }
        }

        private async void PostEvictionCallBack(object cacheKey, object cacheValue, EvictionReason evictionReason, object state)
        {

            await InvokeAsync(() => MudDialog?.CancelAll());

        }

        private bool SearchFilter(TModel element) => SearchFilterFunc(element, _searchString);

        private bool SearchFilterFunc(TModel model, string searchString)
        {
            if (string.IsNullOrEmpty(_searchString)) return true;

            var properties = typeof(TModel).GetProperties().ToList();

            foreach (var prop in properties)
            {
                var columnValue = model.GetPropertyValue(prop.Name);

                if (columnValue is null) continue;
                
                if (columnValue.ToString()!.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            
            return false;
        }
    }
}
