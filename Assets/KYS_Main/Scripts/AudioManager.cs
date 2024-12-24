using InnerSight_Seti;
using UnityEngine;
using UnityEngine.Audio;

namespace InnerSight_Kys
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private AudioMixerGroup audioMixerGroupBgm;
        [SerializeField] private AudioMixerGroup audioMixerGroupSfx;
        public Sounds[] sounds;

        private string bgmSound = ""; // Currently playing BGM
        public string BgmSound => bgmSound;

        protected override void Awake()
        {
            //Singletone 구현부
            base.Awake();

            //AudioMixer 그룹 찾아오기
            AudioMixerGroup[] audioMixerGroups = audioMixer.FindMatchingGroups("Master");

            //AudioManager 초기화
            foreach (var sound in sounds)
            {
                sound.source = this.gameObject.AddComponent<AudioSource>();

                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;

                if (sound.loop)
                {
                    sound.source.outputAudioMixerGroup = audioMixerGroups[1];   //BGM
                }
                else
                {
                    sound.source.outputAudioMixerGroup = audioMixerGroups[2];   //SFX
                }
            }


            //base.Awake();

            //if (audioMixer == null)
            //{
            //    return;
            //}

            //foreach (var sound in sounds)
            //{
            //    // Initialize the AudioSource for each sound
            //    var outputGroup = sound.loop ? audioMixerGroupBgm : audioMixerGroupSfx;
            //    sound.Initialize(this.gameObject, outputGroup);
            //}
        }

        public void Play(string name)
        {
            Sounds sound = System.Array.Find(sounds, s => s.name == name);
            if (sound == null)
            {
                return;
            }
            sound.source.Play();
        }

        public void Stop(string name)
        {
            Sounds sound = System.Array.Find(sounds, s => s.name == name);
            if (sound == null)
            {
                return;
            }
            sound.source.Stop();
            if (name == bgmSound)
            {
                bgmSound = "";
            }
        }

        public void PlayBgm(string name)
        {
            if (bgmSound == name) return;

            StopBgm();

            Sounds sound = System.Array.Find(sounds, s => s.name == name);
            if (sound == null)
            {
                return;
            }

            bgmSound = name;
            sound.source.Play();
        }

        public void StopBgm()
        {
            Stop(bgmSound);
        }
    }
}
