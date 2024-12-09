using System.Collections.Generic;
using UnityEngine;

namespace cdvproject.UI
{
    [CreateAssetMenu(fileName = "InventorySaveData", menuName = "Inventory/Save Data", order = 1)]
    public class InventorySaveData : ScriptableObject
    {
        public List<ItemData> Items = new List<ItemData>();

        public void AddItemData(ItemData data)
        {
            Items.Add(data);
        }
    }
}