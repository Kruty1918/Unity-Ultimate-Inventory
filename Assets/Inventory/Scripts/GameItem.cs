using Sirenix.OdinInspector;
using UnityEngine;

namespace cdvproject.UI
{
    [CreateAssetMenu(menuName = "cdvproject/GameItem", fileName = "New GameItem")]
    public class GameItem : ScriptableObject
    {
        [ReadOnly]
        public string ItemName;

        [PreviewField(64, ObjectFieldAlignment.Left)]
        public Sprite ItemSprite;

        public bool IsStacking = true;

        public void OnValidate()
        {
            ItemName = name;
        }
    }
}