using System.Collections.Generic;

namespace cdvproject.UI
{
    public interface IInventoryPersistence
    {
        void Save(List<ItemContainer> items);
        List<ItemData> Load();
    }
}
