using System.Collections.Generic;
using UnityEngine;
using SGS29.Utilities;

namespace cdvproject.UI
{
    public interface ISlotManager
    {
        void Initialize();
        void AddSlot(InventorySlot slot);
        InventorySlot GetEmptySlot();
        void ClearSlots();
        List<InventorySlot> GetSlots();
    }

    public interface IItemManager
    {
        void AddItem(GameItem item);
        void RemoveItem(ItemContainer item);
        List<ItemContainer> GetItems();
    }

    public interface IInventoryPersistence
    {
        void Save(List<ItemContainer> items);
        List<ItemData> Load();
    }

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
    }

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

    public class InventoryPersistence : IInventoryPersistence
    {
        private const string INVENTORY_SAVE_PATH = "InventorySaveData";

        public void Save(List<ItemContainer> items)
        {
            InventorySaveData saveData = ScriptableObject.CreateInstance<InventorySaveData>();

            for (int i = 0; i < items.Count; i++)
            {
                saveData.AddItemData(items[i].GetItemData(i));
            }

            string savePath = "Assets/Resources/InventorySaveData.asset";
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.CreateAsset(saveData, savePath);
            UnityEditor.AssetDatabase.SaveAssets();
#else
            Debug.LogError("Asset creation is not supported outside the Unity Editor.");
#endif
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

    public class Inventory : MonoSingleton<Inventory>
    {
        [SerializeField] private InventorySlot slotPrefab;
        [SerializeField] private ItemContainer containerPrefab;
        [SerializeField] private Transform parent;
        [SerializeField] private bool useFixedSlots;
        [SerializeField] private int startSlots = 0;

        private ISlotManager slotManager;
        private IItemManager itemManager;
        private IInventoryPersistence persistence;
        private ISlotCreationStrategy slotCreation;
        private ISlotFactory<InventorySlot> slotFactory;

        protected override void Awake()
        {
            base.Awake();

            slotFactory = new SlotCreationFactory(slotPrefab, parent);
            slotCreation = new FixedSlotCreationStrategy(startSlots, slotFactory, this);
            slotManager = new SlotManager(parent, slotCreation, useFixedSlots);
            itemManager = new ItemManager(slotManager, containerPrefab);
            persistence = new InventoryPersistence();
        }

        private void Start()
        {
            Initialize();
            LoadInventory();
        }

        private void Initialize()
        {
            slotManager.Initialize();
        }

        public void AddItem(GameItem item)
        {
            itemManager.AddItem(item);
            SaveInventory();
        }

        public void RemoveItem(ItemContainer item)
        {
            itemManager.RemoveItem(item);
            SaveInventory();
        }

        public void AddSlot(InventorySlot slot)
        {
            slotManager.AddSlot(slot);
        }

        public void SaveInventory()
        {
            persistence.Save(itemManager.GetItems());
        }

        private void LoadInventory()
        {
            if (slotManager.GetSlots().Count == 0)
            {
                Debug.LogWarning("No slots initialized. Initializing default slots.");
            }

            var itemsData = persistence.Load();
            foreach (var itemData in itemsData)
            {
                GameItem gameItem = Resources.Load<GameItem>($"GameItems/{itemData.ItemName}");
                if (gameItem != null)
                {
                    AddItem(gameItem);
                }
                else
                {
                    Debug.LogError($"Item '{itemData.ItemName}' not found.");
                }
            }
        }
    }
}
