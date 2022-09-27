using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudXComponents.Enums;
using MudXComponents.Extensions;
using MudXComponents.Interfaces;

namespace MudXComponents.Components
{
     

    public partial class MudGridX<TModel> : UIMudBase<TModel> where TModel : new()
    {
        [CascadingParameter(Name = nameof(ParentContext))]
        public  object ParentContext { get; set; }

        [Parameter,AllowNull]
        public bool EnableModelValidation { get; set; } 
        //private MudTable<TModel> MyTable { get; set; }

        private bool isFirstRender = true;

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

      

        #region Dialog

        [Inject]
        protected IDialogService DialogService { get; set; }

        protected DialogResult DialogResult { get; set; }

        public DialogParameters Parameters { get; set; } = new DialogParameters();

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

 
                    (string, RenderFragment) DetailGridParameter = new(nameof(MudXPage<TModel>.DetailGrid), DetailGrid);


                    (string, ViewState) crudStateParameter = new (nameof(MudXPage<TModel>.ViewState),button.ViewState);
                    (string, string) TitleParameter = new(nameof(MudXPage<TModel>.DialogTitle), button.Title);



                    button.OnClick = EventCallback.Factory.Create<TModel>(this, callback: async () => await ShowPage<MudXPage<TModel>>(CurrentModel,button.PageSize, crudStateParameter,TitleParameter,DetailGridParameter));
                }
            }

             Components.Add(pChildComponent);

        }

 
        #endregion RenderFragments

       

        protected virtual TModel OnClose()
        {
            return !DialogResult.Cancelled ? ViewModel : default;
        }

        protected virtual async Task ShowPage<TPage>(TModel viewModel, MaxWidth PageSize, params (string, object)[] parameters) where TPage : UIBase
        {

            var stringifiedModel = JsonSerializer.Serialize(viewModel);

            var cloned = JsonSerializer.Deserialize<TModel>(stringifiedModel);

            var newParamList = parameters.ToList();

            newParamList.Add((nameof(ViewModel), cloned));
            newParamList.Add((nameof(MudXPage<TModel>.Components), Components));
            newParamList.Add((nameof(MudXPage<TModel>.OnCreate), OnCreate));
            newParamList.Add((nameof(MudXPage<TModel>.OnUpdate), OnUpdate));
            newParamList.Add((nameof(MudXPage<TModel>.OnDelete), OnDelete));
            newParamList.Add((nameof(MudXPage<TModel>.EnableModelValidation), EnableModelValidation));


            Options.MaxWidth = PageSize;


            var result = await ShowDialogAsync<TPage>(newParamList.ToArray());
 

            Parameters = new DialogParameters();
        }

        protected virtual  Task ShowPage<TPage>(TModel viewModel, params (string, object)[] parameters) where TPage : UIBase
        {
            return ShowPage<TPage>(viewModel, MaxWidth.Medium, parameters);
        }
 
     
        public virtual async ValueTask<TModel> ShowDialogAsync<TPage>(params (string key, object value)[] additionalParameters) where TPage : UIBase
        {
            // if (typeof(TPage) == typeof(BlankPage)) return default;

            Parameters = new DialogParameters();
            //{
            //    { nameof(ViewModel), model }
            //};


            foreach (var prm in additionalParameters)
            {
                Parameters.Add(prm.key, prm.value);
            }

            var dialogTItle = additionalParameters.FirstOrDefault(x => x.key.Equals(nameof(MudXPage<TModel>.DialogTitle)));

            var dialogReference = DialogService.Show<TPage>(dialogTItle.value?.ToString(), Parameters, Options);

            DialogResult = await dialogReference.Result;

            return (TModel)DialogResult.Data;
        }

        //protected void UpdateResponseData(TModel updatedItem)
        //{
        //    UpdateResponseData(DataSource, updatedItem);
        //}

        //protected void UpdateResponseData(IList<TModel> collection, TModel updatedItem)
        //{
        //    var primaryKey = typeof(TModel).GetKey();

        //    var itemToUpdate = collection.FirstOrDefault(x =>
        //        primaryKey != null && x.GetType().GetProperty(primaryKey)!.GetValue(x)!
        //            .Equals(updatedItem.GetType().GetProperty(primaryKey)?.GetValue(updatedItem)));

        //    if (itemToUpdate is null) return;

        //    var index = collection.IndexOf(itemToUpdate);

        //    collection[index] = updatedItem;
        //}

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
