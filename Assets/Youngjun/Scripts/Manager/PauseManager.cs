using InnerSight_Kys;
using UnityEngine;

namespace Noah
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField] private GameObject pauseUI;

        private void Update()
        {
            Pause();
        }

        void Pause()
        {
            if (InputActManager.Instance.IsPauseButtonDown() && !pauseUI.activeSelf)
            {
                AudioManager.Instance.Play("Throw");

                pauseUI.SetActive(true);

                ResetManager.Instance.Pause();
            }
            else if (InputActManager.Instance.IsPauseButtonDown() && pauseUI.activeSelf)
            {
                AudioManager.Instance.Play("Throw");

                pauseUI.SetActive(false);

                ResetManager.Instance.ResetPause();
            }
        }

        public void Resume()
        {
            pauseUI.SetActive(false);
            ResetManager.Instance.ResetPause();
        }

        public void MainMenu()
        {
            pauseUI.SetActive(false);
            ResetManager.Instance.ResetPause();

            SceneFade.instance.FadeOut("MainMenu");
        }


    }

}
