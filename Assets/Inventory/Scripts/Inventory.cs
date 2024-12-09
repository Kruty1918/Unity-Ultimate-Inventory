using UnityEngine;
using SGS29.Utilities;

namespace cdvproject.UI
{
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
            if (item == null)
            {
                Debug.LogError("Cannot add a null item to the inventory.");
                return;
            }

            itemManager.AddItem(item);
            SaveInventory();
        }

        public void RemoveItem(ItemContainer item)
        {
            if (item == null)
            {
                Debug.LogError("Cannot remove a null item from the inventory.");
                return;
            }

            itemManager.RemoveItem(item);
            SaveInventory();
        }

        public void AddSlot(InventorySlot slot)
        {
            if (slot == null)
            {
                Debug.LogError("Cannot add a null slot to the inventory.");
                return;
            }

            slotManager.AddSlot(slot);
        }

        public void SaveInventory()
        {
            try
            {
                persistence.Save(itemManager.GetItems());
                Debug.Log("Inventory successfully saved.");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to save inventory: {ex.Message}");
            }
        }

        public int SlotIndexOf(InventorySlot slot)
        {
            if (slot == null)
            {
                Debug.LogError("Cannot retrieve index of a null slot.");
                return -1;
            }

            return slotManager.SlotIndexOf(slot);
        }

        private void LoadInventory()
        {
            if (slotManager.GetSlots().Count == 0)
            {
                Debug.LogWarning("No slots initialized. Initializing default slots.");
            }

            var itemsData = persistence.Load();
            if (itemsData == null || itemsData.Count == 0)
            {
                Debug.LogWarning("No saved inventory data found.");
                return;
            }

            foreach (var itemData in itemsData)
            {
                GameItem gameItem = Resources.Load<GameItem>($"GameItems/{itemData.ItemName}");
                if (gameItem != null)
                {
                    itemManager.AddItem(gameItem);
                }
                else
                {
                    Debug.LogError($"Item '{itemData.ItemName}' not found in Resources.");
                }
            }
        }
    }
}
