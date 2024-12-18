using InnerSight_Seti;
using UnityEngine;

namespace Noah
{
    public class GameOverManager : Singleton<GameOverManager>
    {
        [SerializeField] private GameObject playerMove;
        [SerializeField] private GameObject gameOverUI;
        [SerializeField] private string playScene = "PlayScene";

        public int fullCount = 30;

        private Transform house;

        public void NPCFull(int count)
        {
            if (count >= fullCount)
            {
                if (!gameOverUI.activeSelf)
                {
                    gameOverUI.SetActive(true);
                }
 
            }
        }

        public void CurrentGoldCheck()
        {
            if (PlayerStats.Instance.CurrentGold < 0)
            {
                if (!gameOverUI.activeSelf)
                {
                    playerMove.SetActive(false);
                    gameOverUI.SetActive(true);              
                }

            }
        }

        public void ReTry()
        {
            playerMove.SetActive(false);
            SceneFade.instance.FadeOut(playScene, 1f);
        }

        public void MainMenu()
        {
            SceneFade.instance.FadeOut("MainMenu");
        }
    }


}
