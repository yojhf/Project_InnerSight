using System.Collections.Generic;
using UnityEngine;

namespace InnerSight_Seti
{
    // 아이템 데이터베이스 ScriptableObject
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/ItemDatabase")]
    public class ItemDatabase : ScriptableObject
    {
        // 아이템 데이터베이스 리스트
        public List<ItemKey> itemList;
    }
}