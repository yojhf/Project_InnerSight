using UnityEngine;
using UnityEngine.Audio;

namespace InnerSight_Yoon
{

    public class PlayBGM : MonoBehaviour
    {
        #region Variables


        private AudioManager audioManager;

        //Audio
        public AudioMixer audioMixer;

        #endregion

        void Start()
        {
            
            audioManager = AudioManager.Instance;

            audioManager.PlayBgm("MenuBGM");

        }
    }

}