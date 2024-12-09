using UnityEngine;

namespace cdvproject.UI
{
    [System.Serializable]
    public class ItemData
    {
        [SerializeField] private string itemName;  // Назва предмета
        [SerializeField] private int quantity;    // Кількість предметів
        [SerializeField] private int slotIndex;   // Індекс слоту в інвентарі

        public string ItemName => itemName;
        public int Quantity => quantity;
        public int SlotIndex => slotIndex;

        public ItemData(string itemName, int quantity, int slotIndex)
        {
            this.itemName = itemName;
            this.quantity = quantity;
            this.slotIndex = slotIndex;
        }

        /// <summary>
        /// Збільшує кількість предметів на вказану величину.
        /// </summary>
        public void AddQuantity(int amount)
        {
            quantity += amount;
        }

        /// <summary>
        /// Встановлює кількість предметів.
        /// </summary>
        public void SetQuantity(int newQuantity)
        {
            quantity = newQuantity;
        }
    }
}