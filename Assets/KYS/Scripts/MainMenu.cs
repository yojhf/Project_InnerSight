//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.Audio;
//using InnerSight_Seti;
//using Noah;

//namespace InnerSight_Kys
//{
//    public class MainMenu : MonoBehaviour
//    {
//        #region Variables
//        public SceneFader fader;
//        [SerializeField] private string loadToScene = "MainScene01";

//        //private AudioManager audioManager;

//        public GameObject mainMenuUI;
//        public GameObject optionUI;
//        public GameObject creditUI;
//        public GameObject loadGameButton;

//        //Audio
//        public AudioMixer audioMixer;
//        public Slider bgmSlider;
//        public Slider sfxSlider;

//        //����Ǿ� �ִ� ����ȣ
//        //private int sceneNumber;
//        #endregion

//        private void Start()
//        {
//            //���� ������ �ʱ�ȭ
//            InitGameData();
//            //����� ���� ������
//            if (PlayerStats.Instance.SceneNumber > 0)
//            {
//                loadGameButton.SetActive(true);
//            }

//            //�� ���̵��� ȿ��
//            fader.FromFade();

//            //����
//            audioManager = AudioManager.Instance;

//            //Bgm �÷���
//            audioManager.PlayBgm("MenuBgm");
//        }

//        private void InitGameData()
//        {
//            //���Ӽ�����: ����� �ɼǰ� �ҷ�����
//            LoadOptions();

//            //���� �÷��� ������ �ε�
//            PlayData playData = SaveLoad.LoadData();
//            PlayerStats.Instance.PlayerStatInit(playData);
//        }

//        public void NewGame()
//        {
//            //���� ������ �ʱ�ȭ
//            PlayerStats.Instance.PlayerStatInit(null);

//            audioManager.Stop(audioManager.BgmSound);
//            audioManager.Play("MenuButton");

//            fader.FadeTo(loadToScene);
//        }

//        public void LoadGame()
//        {
//            //Debug.Log($"Goto LoadGame {sceneNumber}�� ��");
//            audioManager.Stop(audioManager.BgmSound);
//            audioManager.Play("MenuButton");

//            fader.FadeTo(PlayerStats.Instance.SceneNumber);
//        }

//        public void Options()
//        {
//            audioManager.Play("MenuButton");

//            ShowOptions();
//        }

//        public void Credits()
//        {
//            ShowCredit();
//        }

//        public void QuitGame()
//        {
//            //Cheating
//            PlayerPrefs.DeleteAll();

//            Debug.Log("Quit Game");
//            Application.Quit();
//        }

//        private void ShowOptions()
//        {
//            audioManager.Play("MenuButton");

//            mainMenuUI.SetActive(false);
//            optionUI.SetActive(true);
//        }

//        public void HideOptions()
//        {
//            //�ɼǰ� �����ϱ�
//            SaveOptions();

//            optionUI.SetActive(false);
//            mainMenuUI.SetActive(true);
//        }

//        //AudioMixer Bgm -40~0
//        public void SetBgmVolume(float value)
//        {
//            audioMixer.SetFloat("BgmVolume", value);
//        }

//        //AudioMixer Sfx -40~0
//        public void SetSfxVolume(float value)
//        {
//            audioMixer.SetFloat("SfxVolume", value);
//        }

//        //�ɼǰ� �����ϱ�
//        private void SaveOptions()
//        {
//            PlayerPrefs.SetFloat("BgmVolume", bgmSlider.value);
//            PlayerPrefs.SetFloat("SfxVolume", sfxSlider.value);
//        }

//        //�ɼǰ� �ε��ϱ�
//        private void LoadOptions()
//        {
//            //����� ����
//            float bgmVolume = PlayerPrefs.GetFloat("BgmVolume", 0);
//            //Debug.Log($"bgmVolume: {bgmVolume}");
//            SetBgmVolume(bgmVolume);        //���� ���� ����
//            bgmSlider.value = bgmVolume;    //UI ����

//            //ȿ���� ����
//            float sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 0);
//            //Debug.Log($"sfxVolume: {sfxVolume}");
//            SetSfxVolume(sfxVolume);        //���� ���� ����
//            sfxSlider.value = sfxVolume;    //UI ����

//            //��Ÿ...
//        }

//        private void ShowCredit()
//        {
//            mainMenuUI.SetActive(false);
//            creditUI.SetActive(true);
//        }
//    }
//}