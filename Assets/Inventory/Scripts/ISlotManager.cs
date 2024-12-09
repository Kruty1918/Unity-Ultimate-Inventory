using System.Collections.Generic;

namespace cdvproject.UI
{
    public interface ISlotManager
    {
        void Initialize();
        void AddSlot(InventorySlot slot);
        InventorySlot GetEmptySlot();
        void ClearSlots();
        List<InventorySlot> GetSlots();
        int SlotIndexOf(InventorySlot slot);
    }
}