namespace cdvproject.UI
{
    public class FixedSlotCreationStrategy : ISlotCreationStrategy
    {
        private int fixedSlotCount;
        private ISlotFactory<InventorySlot> slotFactory;
        private Inventory inventory;

        public FixedSlotCreationStrategy(int slotCount, ISlotFactory<InventorySlot> slotFactory, Inventory inventory)
        {
            fixedSlotCount = slotCount;
            this.slotFactory = slotFactory;
            this.inventory = inventory;
        }

        public void CreateSlots()
        {
            for (int i = 0; i < fixedSlotCount; i++)
            {
                InventorySlot slot = CreateSlot();
                inventory.AddSlot(slot);
            }
        }

        private InventorySlot CreateSlot()
        {
            return slotFactory.CreateSlot();
        }
    }
}