using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace cdvproject.UI
{
    public class InventoryPersistence : IInventoryPersistence
    {
        private const string INVENTORY_SAVE_PATH = "InventorySaveData";

        public void Save(List<ItemContainer> items)
        {
            string savePath = "Assets/Resources/InventorySaveData.asset";
            string directoryPath = Path.GetDirectoryName(savePath);

#if UNITY_EDITOR
            // Перевіряємо, чи існує директорія
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath); // Створюємо директорію, якщо її немає

                UnityEditor.AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<InventorySaveData>(), savePath);
                UnityEditor.AssetDatabase.SaveAssets();
            }
#endif

            InventorySaveData saveData = Resources.Load<InventorySaveData>("InventorySaveData");

            for (int i = 0; i < items.Count; i++)
            {
                saveData.AddItemData(items[i].GetItemData());
            }
        }

        public List<ItemData> Load()
        {
            InventorySaveData saveData = Resources.Load<InventorySaveData>(INVENTORY_SAVE_PATH);

            if (saveData == null)
            {
                Debug.LogWarning("No save data found.");
                return new List<ItemData>();
            }

            return saveData.Items;
        }
    }
}
