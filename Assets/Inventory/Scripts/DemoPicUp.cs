using System.Collections.Generic;
using cdvproject.UI;
using SGS29.Utilities;
using UnityEngine;

namespace cdvproject.Demo
{
    public class DemoPicUp : MonoBehaviour
    {
        [SerializeField] private List<GameItem> items = new List<GameItem>();

        public void RandomPicUp()
        {
            int randIndex = Random.Range(0, items.Count);
            SM.Instance<Inventory>().AddItem(items[randIndex]);
        }
    }
}