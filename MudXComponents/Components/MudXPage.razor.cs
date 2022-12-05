using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MudXComponents.Enums;
using MudXComponents.Extensions;
using System.Diagnostics.CodeAnalysis;
using MudXComponents.Args;

namespace MudXComponents.Components
{
    public partial class MudXPage<TModel> : UIMudBase<TModel>, IDisposable where TModel : new()
    {
       

        public GridXArgs<TModel> CreateArgs => new GridXArgs<TModel> { Index = Index, OldData = Original, NewData = ViewModel, Page = this, FromChild = FromChild };

        [Parameter]
        public EventCallback<GridXArgs<TModel>> OnSubmit { get; set; }

        [CascadingParameter(Name = nameof(TopLevel))]
        private object _topLevel { get; set; }

        [Parameter, AllowNull]
        public object TopLevel { get => _topLevel; set => _topLevel = value; }

        public bool IsLastPage { get; set; }
        //public  bool Abort { get; set; }

        //public bool Saved { get; set; }



        [Parameter, AllowNull]
        public TModel ViewModel { get; set; }

      
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

        public bool FromChild { get; set; }

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
        public EventCallback ChildSubmit { get; set; } 

        protected override async Task OnInitializedAsync()
        {
            ViewModel = ViewModel.ToImmutable();

            SubmitText = SubmitText.Equals(string.Empty) ? ViewState.ToString() : SubmitText;

            CancelText = CancelText.Equals(string.Empty) ? "Cancel" : CancelText;

            if (!OnSubmit.HasDelegate)
                OnSubmit = EventCallback.Factory.Create<GridXArgs<TModel>>(this, async () => await SubmitClick(CreateArgs));

            if (!IsChild)
                TopLevel = this;
            //else
            //{
            //    ((dynamic)TopLevel).Abort = false;
            //}

            await OnLoad.InvokeAsync(this);

            await base.OnInitializedAsync();
        }

        private object GetTopLevel()
        {
            if (IsChild) return this;

            return TopLevel;
        }
        public async void OnExpire()
        {
            await InvokeAsync(() => MudDialog.CancelAll());
        }

        public virtual void Close()
        {      
            MudDialog.Cancel();
        }

        protected async ValueTask SubmitClick(GridXArgs<TModel> args)
        {
            IsLastPage = true;
            await SubmitClick(args, false); 
        }

        protected async ValueTask SubmitClick(GridXArgs<TModel> args, bool fromChild)
        {
            //if (args.NewData is null) return;

            FromChild = fromChild;

            await OnBeforeSubmit.InvokeAsync(CreateArgs);

            if (ViewState == ViewState.Create)
            {
                if (IsChild && SmartCrud)
                {
                    if (ChildSubmit.HasDelegate)
                    {
                        await ChildSubmit.InvokeAsync();

                        PropertyExtensions.SetParentChildRelation(((dynamic)TopLevel).ViewModel, ViewModel);
                    }
                }

                await OnCreate.InvokeAsync(CreateArgs);
                

            }
            else  if (ViewState == ViewState.Update && !FromChild)
            {
                await OnUpdate.InvokeAsync(CreateArgs);
            }

            else if (ViewState == ViewState.Remove)
            {
                await OnDelete.InvokeAsync(CreateArgs);
            }


            //Şimdilik kaldırılsın, işlem başarılı olursa ekran kapatlsın
            //if(!FromChild)
            //    MudDialog.Close();
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
            await SubmitClick(CreateArgs, true);
            //await ChildSubmit.InvokeAsync();
            //await SubmitClick(CreateArgs);


        }
    }
}

