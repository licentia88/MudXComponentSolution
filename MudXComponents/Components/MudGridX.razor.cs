using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;
using MudBlazor;
using MudXComponents.Args;
using MudXComponents.Enums;
using MudXComponents.Extensions;
using MudXComponents.Interfaces;

namespace MudXComponents.Components;



public partial class MudGridX<TModel> : UIMudBase<TModel> where TModel : new()
{

    [Parameter]
    public string Height { get; set; }  

    [Parameter, AllowNull]
    public bool FixedHeader { get; set; } = true;

    [Parameter, AllowNull]
    public bool FixedFooter { get; set; } = true;

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

    

    #region Page Information

    [Parameter, AllowNull]
    public string Title { get; set; }

    #endregion Page Commands

    [Parameter, AllowNull]
    public virtual EventCallback<GridXArgs<TModel>> OnCreate { get; set; }

    [Parameter, AllowNull]
    public virtual EventCallback<GridXArgs<TModel>> OnDelete { get; set; }

    [Parameter, AllowNull]
    public virtual EventCallback<GridXArgs<TModel>> OnUpdate { get; set; }

    [Parameter, AllowNull]
    public virtual EventCallback<GridXArgs<TModel>> OnBeforeSubmit { get; set; }


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
    public RenderFragment<TModel> DetailGrid { get; set; }

    private string _searchString = "";

    public ObservableCollection<ColumnBase<TModel>> Components { get; set; } = new();

    private bool isFirstRender = true;

    [CascadingParameter(Name =nameof(IsChild))]
    internal  bool IsChild { get; set; }

  

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

    

    internal void ShowDetailAsync(TModel context)
    {
        //if(VisibleDetailKey)
        //if(context.GetPrimaryKeyValue().ToString() == VisibleDetailKey.ToString())

        isClicked = !isClicked;

        if (isClicked)
        {
            VisibleDetailKey = context.GetPrimaryKeyValue();

        }
        else if (VisibleDetailKey.ToString() != context.GetPrimaryKeyValue().ToString())
        {
            VisibleDetailKey = context.GetPrimaryKeyValue().ToString();
            ShowDetailAsync(context);
        }
        else
            VisibleDetailKey = null;


    }

    private bool ShouldDisplay(TModel context)
    {
        if (VisibleDetailKey is null) return false;

        return isClicked && (context.GetPrimaryKeyValue().ToString() == VisibleDetailKey.ToString());
    }

    private bool isClicked;

    private object VisibleDetailKey;



    protected override async Task OnInitializedAsync()
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

        await base.OnInitializedAsync();
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

                // var myParams = this.GetType().GetProperties(System.Reflection.BindingFlags.Public
                //                                            |System.Reflection.BindingFlags.NonPublic
                //                                            |System.Reflection.BindingFlags.Instance)
                //     
                //     .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(ParameterAttribute))).ToList();

                // foreach (var par in myParams)
                // {
                //     paramList.Add((par.Name, this.GetPropertyValue(par.Name)));
                // }

                paramList.Add((nameof(MudXPage<TModel>.Components), Components));
                paramList.Add((nameof(MudXPage<TModel>.OnCreate), OnCreate));
                paramList.Add((nameof(MudXPage<TModel>.OnUpdate), OnUpdate));
                paramList.Add((nameof(MudXPage<TModel>.OnDelete), OnDelete));
                paramList.Add((nameof(MudXPage<TModel>.OnLoad), OnLoad));
                paramList.Add((nameof(MudXPage<TModel>.OnBeforeSubmit), OnBeforeSubmit));

                paramList.Add((nameof(MudXPage<TModel>.EnableModelValidation), EnableModelValidation));
                paramList.Add((nameof(MudXPage<TModel>.DetailGrid), DetailGrid));
                
                paramList.Add((nameof(MudXPage<TModel>.IsChild), IsChild));
                paramList.Add((nameof(MudXPage<TModel>.SmartCrud), SmartCrud));
                paramList.Add((nameof(MudXPage<TModel>.ParentContext), ParentContext));

                paramList.Add((nameof(MudXPage<TModel>.ParentGrid), this));
                paramList.Add((nameof(MudXPage<TModel>.ViewState), button.ViewState));
                paramList.Add((nameof(MudXPage<TModel>.DialogTitle), button.Title));
                //ParentGrid
                button.OnClick = EventCallback.Factory.Create<TModel>(this, callback: async () => await ShowDialogAsync<MudXPage<TModel>>(CurrentModel,button.PageSize, paramList.ToArray()));

                
            }
        }

         Components.Add(pChildComponent);

    }


    #endregion RenderFragments

   

    //protected virtual TModel OnClose()
    //{
    //    return !DialogResult.Cancelled ? ViewModel : default;
    //}

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

        newParamList.Add((nameof(MudXPage<TModel>.ViewModel), cloned));

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

        newParamList.Add((nameof(MudXPage<TModel>.Index), DataSource.IndexOf(viewModel)));
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

        var searchableFields = Components.Where(x => x.BindingField is not null).Select(x => x.BindingField);

        //var properties = typeof(TModel).GetProperties().ToList();

        foreach (var fieldName in searchableFields)
        {
            var columnValue = model.GetPropertyValue(fieldName);

            if (columnValue is null) continue;
            
            if (columnValue.ToString()!.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
        }
        
        return false;
    }

    
}
