using UnityEngine;

namespace InnerSight_Seti
{
    public class Item : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private int itemID = 0; // 고유한 ID로 데이터베이스 내 아이템 참조
        private ItemKey itemData;
        public ItemDatabase itemDatabase; // 데이터베이스 참조

        public int testnum;

        public int ItemId => itemID;

        private void Awake()
        {
            // ID를 사용해서 ItemDatabase에서 아이템 데이터를 가져옴
            itemData = itemDatabase.itemList.Find(item => item.itemID == itemID);
        }

        public ItemKey GetItemData()
        {
            return itemData;
        }

        public int test()
        { 
            return testnum = Random.Range(GetItemData().itemPrice - 20, testnum);
        }
    }
}