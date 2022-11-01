using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MudXComponents.Enums;
using MudXComponents.Extensions;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics.CodeAnalysis;
using MudXComponents.Args;

namespace MudXComponents.Components
{
    public partial class MudXPage<TModel> : UIMudBase<TModel>, IDisposable where TModel : new()
    {
       

        private GridXArgs<TModel> CreateArgs => new GridXArgs<TModel> { Index = Index, OldData = Original, NewData = ViewModel, Page = this };

        [Parameter]
        public EventCallback<GridXArgs<TModel>> OnSubmit { get; set; }

        [CascadingParameter(Name = nameof(TopLevel))]
        private object _topLevel { get; set; }

        [Parameter, AllowNull]
        public object TopLevel { get => _topLevel; set => _topLevel = value; }


        [Parameter, AllowNull]
        public TModel ViewModel { get; set; }

        [Inject]
        public IMemoryCache MemoryCache { get; set; }

        [Parameter]
        public TModel Original { get; set; }

        [Parameter]
        public int Index { get; set; }

        [Parameter, EditorBrowsable(EditorBrowsableState.Never)]
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
        public bool SmartCrud { get; set; }

        [Parameter, EditorBrowsable(EditorBrowsableState.Never)]
        public RenderFragment<TModel> DetailGrid { get; set; }

        [Parameter]
        public ObservableCollection<ColumnBase<TModel>> Components { get; set; }

        [Parameter]
        public EventCallback<GridXArgs<TModel>> OnCreate { get; set; }

        [Parameter]
        public EventCallback<GridXArgs<TModel>> OnUpdate { get; set; }

        [Parameter]
        public EventCallback<GridXArgs<TModel>> OnDelete { get; set; }

        [Parameter]
        public EventCallback<GridXArgs<TModel>> OnBeforeSubmit { get; set; }

        [Parameter]
        public EventCallback<MudXPage<TModel>> OnLoad { get; set; }

        [Parameter]
        public string CacheKey { get; set; }

        protected override async Task OnInitializedAsync()
        {

            ViewModel = ViewModel.ToImmutable();

            //CacheKey = MemoryCache.Get<string>(nameof(CacheKey));

            SubmitText = SubmitText.Equals(string.Empty) ? ViewState.ToString() : SubmitText;

            CancelText = CancelText.Equals(string.Empty) ? "Cancel" : CancelText;

            if (!OnSubmit.HasDelegate)
                OnSubmit = EventCallback.Factory.Create<GridXArgs<TModel>>(this, async () => await SubmitClick(CreateArgs));

            if (!IsChild)
                TopLevel = this;

            await OnLoad.InvokeAsync(this);

            await base.OnInitializedAsync();
        }

        public async void OnExpire()
        {
            await InvokeAsync(() => MudDialog.CancelAll());
        }

        public virtual void Close()
        {
            //ViewModel = Original;
            MudDialog.Cancel();
        }

        protected async ValueTask SubmitClick(GridXArgs<TModel> args, bool FromChild = false)
        {
            if (args.NewData is null) return;

            await OnBeforeSubmit.InvokeAsync(CreateArgs);
            var KeyValue = ParentContext?.GetPrimaryKeyValue();

            if (ViewState == ViewState.Create)
            {
                if (IsChild && SmartCrud)
                {
                   
                    if (KeyValue.IsNullOrDefault())
                    {
                        await ((dynamic)TopLevel).SubmitFromChild();

                        PropertyExtensions.SetParentChildRelation(((dynamic)TopLevel).ViewModel, ViewModel);

                        
                    }

                    await OnCreate.InvokeAsync(CreateArgs);

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
                 
                    await OnUpdate.InvokeAsync(CreateArgs);

                }
                else
                {
                    await OnUpdate.InvokeAsync(CreateArgs);
                }


            }
            else if (ViewState == ViewState.Remove)
            {
                await OnDelete.InvokeAsync(CreateArgs);
            }
            else
            {
                return;
            }

            if(!FromChild)
                MudDialog.Close();
        }

        /// <summary>
        /// Gets component by bindingfield
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public TComponent GetComponent<TComponent>(string FieldName) where TComponent : ColumnBase<TModel>
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

        public async Task SubmitFromChild()
        {
               ViewState = ViewState.Create;
               await SubmitClick(CreateArgs,true);

              ViewState = ViewState.Update;
        }
    }
}

