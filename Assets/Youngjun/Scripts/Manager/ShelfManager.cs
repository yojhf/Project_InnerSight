using InnerSight_Seti;
using System.Collections.Generic;
using UnityEngine;

namespace Noah
{
    public class ShelfManager : Singleton<ShelfManager>
    {
        private List<ShelfStorage> shelfs = new List<ShelfStorage>();

        private void Start()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                shelfs.Add(transform.GetChild(i).GetComponent<ShelfStorage>());
            }
        }

        public void ActiveShelf(Item item)
        {
            foreach (var itemId in shelfs)
            {
                if (itemId.keyId == item.ItemId)
                {
                    itemId.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

}
