using UnityEngine;

namespace cdvproject.UI
{
    public interface IDragger
    {
        void SetParentAfterDrag(Transform transform);
        ItemContainer itemContainer { get; }
    }
}