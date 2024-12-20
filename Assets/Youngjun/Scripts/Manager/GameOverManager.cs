using InnerSight_Kys;
using InnerSight_Seti;
using UnityEngine;

namespace Noah
{
    public class GameOverManager : Singleton<GameOverManager>
    {
        [SerializeField] private GameObject dayInfo;
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
                    AudioManager.Instance.Play("GameOver");
                    gameOverUI.SetActive(true);
                    playerMove.SetActive(false);
                    dayInfo.SetActive(false);
                }
 
            }
        }

        public void CurrentGoldCheck()
        {
            if (PlayerStats.Instance.CurrentGold < 0)
            {
                if (!gameOverUI.activeSelf)
                {
                    AudioManager.Instance.Play("GameOver");
                    playerMove.SetActive(false);
                    gameOverUI.SetActive(true);
                    dayInfo.SetActive(false);
                }

            }
        }

        public void ReTry()
        {
            AudioManager.Instance.Play("BtnClick");
            playerMove.SetActive(false);
            dayInfo.SetActive(true);
            SceneFade.instance.FadeOut(playScene);
        }

        public void MainMenu()
        {
            AudioManager.Instance.Play("BtnClick");
            dayInfo.SetActive(true);
            SceneFade.instance.FadeOut("MainMenu");
        }
    }


}
