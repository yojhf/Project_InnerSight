using System.Collections.Generic;
using UnityEngine;

namespace InnerSight_Seti
{
    // ������ �����ͺ��̽� ScriptableObject
    [CreateAssetMenu(fileName = "Database_NPC", menuName = "Database/Database_NPC")]
    public class Database_NPC : ScriptableObject
    {
        // ������ �����ͺ��̽� ����Ʈ
        public List<Key_NPC> NPC_List;
    }
}