using UnityEngine;

namespace InnerSight_Seti
{
    /// <summary>
    /// NPC의 형식을 지정하는 데이터 컨테이너 클래스
    /// </summary>
    [System.Serializable]
    public class Key_NPC
    {
        public int NPC_ID;
        public string NPC_Name;
        public string NPC_Description;
        public GameObject NPC_Prefab;
    }
}