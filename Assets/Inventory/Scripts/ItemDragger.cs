using SGS29.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace cdvproject.UI
{
    public class ItemDragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDragger
    {
        private ItemContainer itemContainer;
        private Transform parentAfterDrag;

        ItemContainer IDragger.itemContainer => itemContainer;

        void Start()
        {
            SetContainer(GetComponent<ItemContainer>());
        }

        public void SetContainer(ItemContainer container)
        {
            itemContainer = container;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            itemContainer.SetRaycastTarget(false);
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            itemContainer.SetRaycastTarget(true);
            transform.SetParent(parentAfterDrag);
            ResetPosition();
        }

        public void SetParentAfterDrag(Transform transform)
        {
            parentAfterDrag = transform;
        }

        public void ResetPosition()
        {
            transform.localPosition = Vector3.zero;
        }

        public ItemContainer GetContainer() => itemContainer;
    }
}