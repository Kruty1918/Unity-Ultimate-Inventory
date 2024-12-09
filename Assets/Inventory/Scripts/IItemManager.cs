using System.Collections.Generic;

namespace cdvproject.UI
{
    public interface IItemManager
    {
        void AddItem(GameItem item);
        void RemoveItem(ItemContainer item);
        List<ItemContainer> GetItems();
    }
}
