using InnerSight_Seti;
using UnityEngine;

namespace Noah
{
    public class GameOverManager : Singleton<GameOverManager>
    {
        public int fullCount = 30;

        private Transform house;

        public void NPCFull(int count)
        {
            if (count >= fullCount)
            {
                Debug.Log("게임오버");
            }
        }

        public void CurrentGold()
        {
            if (PlayerStats.Instance.CurrentGold < 0)
            {
                Debug.Log("게임오버");
            }
        }
    }


}
