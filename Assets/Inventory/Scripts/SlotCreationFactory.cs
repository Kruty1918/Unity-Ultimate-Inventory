using UnityEngine;

namespace cdvproject.UI
{
    public class SlotCreationFactory : ISlotFactory<InventorySlot>
    {
        private InventorySlot slotPrefab;
        private Transform parent;

        public SlotCreationFactory(InventorySlot slotPrefab, Transform parent)
        {
            this.slotPrefab = slotPrefab;
            this.parent = parent;
        }

        public InventorySlot CreateSlot()
        {
            return GameObject.Instantiate(slotPrefab, parent);
        }
    }
}