using System.Collections.ObjectModel;

namespace MudXComponents.Extensions;

public static class CollectionHelpers
{
    public static void UpdateCollection<TModel>(this Collection<TModel> collection, TModel newValue)
    {
        var primaryKey = typeof(TModel).GetKey();

        var itemToUpdate = collection.FirstOrDefault(x =>
            primaryKey != null && x.GetType().GetProperty(primaryKey)!.GetValue(x)!
                .Equals(newValue.GetType().GetProperty(primaryKey)?.GetValue(newValue)));

        if (itemToUpdate is null) return;

        var index = collection.IndexOf(itemToUpdate);

        collection[index] = newValue;
    }

    public static void RemoveFromCollection<TModel>(this Collection<TModel> collection, TModel newValue)
    {
        var primaryKey = typeof(TModel).GetKey();

        var itemToUpdate = collection.FirstOrDefault(x =>
            primaryKey != null && x.GetType().GetProperty(primaryKey)!.GetValue(x)!
                .Equals(newValue.GetType().GetProperty(primaryKey)?.GetValue(newValue)));

        if (itemToUpdate is null) return;

        var index = collection.IndexOf(itemToUpdate);

        collection.RemoveAt(index);
        //collection[index] = newValue;
    }


}
