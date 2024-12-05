using System.Collections.Generic;
using UnityEngine;

namespace InnerSight_Seti
{
    // 아이템 데이터베이스 ScriptableObject
    [CreateAssetMenu(fileName = "Database_Item", menuName = "Database/Database_Item")]
    public class ItemDatabase : ScriptableObject
    {
        // 아이템 데이터베이스 리스트
        public List<ItemKey> itemList;
    }
}