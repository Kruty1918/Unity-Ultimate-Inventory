using SGS29.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace cdvproject.UI
{
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        public bool IsEmpty() => transform.childCount <= 0;
        public ItemContainer GetItem() => GetComponentInChildren<ItemContainer>();

        public void ClearSlot()
        {
            Destroy(transform.GetChild(0));
        }

        public void OnDrop(PointerEventData eventData)
        {
            IDragger dragger = eventData.pointerDrag.GetComponent<IDragger>();

            if (IsEmpty())
            {
                dragger.SetParentAfterDrag(transform);
            }
            else
            {
                // Якщо слот НЕ порожній, намагаємося виконати логіку стекування
                ItemContainer slotContainer = transform.GetComponentInChildren<ItemContainer>();

                if (dragger.itemContainer.CompareName(slotContainer.GetItemName()))
                {
                    slotContainer.AddStack(dragger.itemContainer.GetQuantity());
                    SM.Instance<Inventory>().RemoveItem(dragger.itemContainer);
                }
            }
        }
    }
}