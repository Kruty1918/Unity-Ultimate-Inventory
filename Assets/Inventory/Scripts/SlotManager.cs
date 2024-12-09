using System.Collections.Generic;
using UnityEngine;

namespace cdvproject.UI
{
    public class SlotManager : ISlotManager
    {
        private List<InventorySlot> slots = new List<InventorySlot>();
        private Transform parent;
        private bool useFixedSlots;
        private ISlotCreationStrategy creationStrategy;

        public SlotManager(Transform parent, ISlotCreationStrategy creationStrategy, bool useFixedSlots)
        {
            this.parent = parent;
            this.useFixedSlots = useFixedSlots;
            this.creationStrategy = creationStrategy;
        }

        public void AddSlot(InventorySlot slot)
        {
            slots.Add(slot);
        }

        public InventorySlot GetEmptySlot()
        {
            foreach (var slot in slots)
            {
                if (slot.IsEmpty())
                {
                    return slot;
                }
            }
            return null;
        }

        public void ClearSlots()
        {
            foreach (var slot in parent.GetComponentsInChildren<InventorySlot>())
            {
                Object.Destroy(slot.gameObject);
            }
            slots.Clear();
        }

        public List<InventorySlot> GetSlots() => slots;

        public void Initialize()
        {
            if (useFixedSlots)
            {
                ClearSlots();
                creationStrategy.CreateSlots();
            }
            else
            {
                slots.AddRange(parent.GetComponentsInChildren<InventorySlot>());
            }
        }

        public int SlotIndexOf(InventorySlot slot)
        {
            return slots.IndexOf(slot);
        }
    }
}
