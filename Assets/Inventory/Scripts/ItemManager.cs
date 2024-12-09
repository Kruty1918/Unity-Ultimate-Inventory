using System.Collections.Generic;
using UnityEngine;

namespace cdvproject.UI
{
    public class ItemManager : IItemManager
    {
        private List<ItemContainer> items = new List<ItemContainer>();
        private ISlotManager slotManager;
        private ItemContainer containerPrefab;

        public ItemManager(ISlotManager slotManager, ItemContainer containerPrefab)
        {
            this.slotManager = slotManager;
            this.containerPrefab = containerPrefab;
        }

        public void AddItem(GameItem item)
        {
            foreach (var itemContainer in items)
            {
                if (itemContainer.CompareName(item.ItemName))
                {
                    itemContainer.AddStack(1);
                    return;
                }
            }

            InventorySlot emptySlot = slotManager.GetEmptySlot();
            if (emptySlot == null)
            {
                Debug.LogError("No empty slots available.");
                return;
            }

            ItemContainer newContainer = Object.Instantiate(containerPrefab, emptySlot.transform);
            newContainer.InitializeItem(item, 1);
            items.Add(newContainer);
        }

        public void RemoveItem(ItemContainer item)
        {
            items.Remove(item);
            Object.Destroy(item.gameObject);
        }

        public List<ItemContainer> GetItems() => items;
    }
}
