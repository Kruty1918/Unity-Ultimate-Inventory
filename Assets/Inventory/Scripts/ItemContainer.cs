using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace cdvproject.UI
{
    public class ItemContainer : MonoBehaviour
    {
        [ReadOnly]
        [SerializeField] private GameItem item;
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI quantityText;

        [ReadOnly]
        [SerializeField] private int quantity;

        void Start()
        {
            if (item.IsStacking)
            {
                quantityText.raycastTarget = false;
                quantityText.gameObject.SetActive(item.IsStacking);
            }
        }

        public void InitializeItem(GameItem item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;

            UpdateUI();
        }

        private void UpdateUI()
        {
            image.sprite = item.ItemSprite;

            if (item.IsStacking)
            {
                quantityText.text = quantity.ToString();
            }
        }

        public void SetRaycastTarget(bool target)
        {
            image.raycastTarget = target;
        }

        public void AddStack(int stack)
        {
            if (stack <= 0) stack = 1;
            quantity += stack;
            UpdateUI();
        }

        public int GetQuantity() => quantity;
        public string GetItemName() => item.ItemName;
        public bool CompareName(string name) => item.ItemName == name;

        /// <summary>
        /// Повертає об'єкт ItemData для серіалізації або збереження даних контейнера.
        /// </summary>
        /// <param name="slotIndex">Індекс слоту, до якого прив'язаний контейнер.</param>
        /// <returns>ItemData, що містить інформацію про предмет.</returns>
        public ItemData GetItemData(int slotIndex)
        {
            return new ItemData(item.ItemName, quantity, slotIndex);
        }
    }
}