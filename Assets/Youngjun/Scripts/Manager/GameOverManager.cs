using InnerSight_Seti;
using UnityEngine;

namespace Noah
{
    public class GameOverManager : Singleton<GameOverManager>
    {
        [SerializeField] private GameObject gameOverUI;
        [SerializeField] private string playScene = "PlayScene";

        public int fullCount = 30;

        private Transform house;

        public void NPCFull(int count)
        {
            if (count >= fullCount)
            {
                if(!gameOverUI.activeSelf)
                    gameOverUI.SetActive(true);
            }
        }

        public void CurrentGold()
        {
            if (PlayerStats.Instance.CurrentGold < 0)
            {
                if (!gameOverUI.activeSelf)
                    gameOverUI.SetActive(true);
            }
        }

        public void ReTry()
        {
            SceneFade.instance.FadeOut(playScene, 1f);
        }

        public void MainMenu()
        {
            Debug.Log("메인메뉴");
        }
    }


}
