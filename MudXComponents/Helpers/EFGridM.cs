using System;
using MudXComponents.Args;
using MudXComponents.Components;

namespace MudXComponents.Helpers
{
	public class EFGridM<TModel> where TModel:new()
	{
        public MudGridX<TModel> GridRef { get; set; }

        public virtual ValueTask CreateAsync(GridXArgs<TModel> args)
        {
            return ValueTask.CompletedTask;
        }

        public virtual ValueTask UpdateAsync(GridXArgs<TModel> args)
        {
            return ValueTask.CompletedTask;
        }

        public virtual ValueTask RemoveAsync(GridXArgs<TModel> args)
        {
            return ValueTask.CompletedTask;
        }

        public virtual void LoadAsync(MudXPage<TModel> page)
        {
             
        }
    }

    public class EFGridD<TModel,TChildModel> : EFGridM<TModel> where TModel:new() where TChildModel:new()
    {

    }
}

