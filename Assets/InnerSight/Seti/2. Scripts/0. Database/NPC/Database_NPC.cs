using System.Collections.Generic;
using UnityEngine;

namespace InnerSight_Seti
{
    // 아이템 데이터베이스 ScriptableObject
    [CreateAssetMenu(fileName = "Database_NPC", menuName = "Database/Database_NPC")]
    public class Database_NPC : ScriptableObject
    {
        // 아이템 데이터베이스 리스트
        public List<Key_NPC> NPC_List;
    }
}