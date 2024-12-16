using UnityEngine;
using UnityEngine.Audio;

namespace InnerSight_Kys
{

    //������� �����ϴ� Ŭ����
    public class AudioManager : SingleTons<AudioManager>
    {
        #region Variables
        public Sound[] sounds;

        // ���� Sound Ŭ���� ����
        [System.Serializable]
        public class Sound
        {
            public string name;
            public AudioClip clip;
            public float volume;
            public float pitch;
            public bool loop;
            [HideInInspector] public AudioSource source;
        }

        private string bgmSound = "";       //���� �÷��� �Ǵ� ����� �̸� 
        public string BgmSound
        {
            get
            {
                return bgmSound;
            }
        }
        public AudioMixer audioMixer;
        #endregion

        protected override void Awake()
        {
            //singletone ������
            base.Awake();

            //AudioMixerGroup ã�ƿ���
            AudioMixerGroup[] audioMixerGroups = audioMixer.FindMatchingGroups("Master");

            //audioManager �ʱ�ȭ
            foreach (var sound in sounds)
            {
                sound.source = this.gameObject.AddComponent<AudioSource>();

                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;

                if (sound.loop)
                {
                    sound.source.outputAudioMixerGroup = audioMixerGroups[1]; //BGM

                }
                else
                {
                    sound.source.outputAudioMixerGroup = audioMixerGroups[2]; //SFX

                }
            }
        }
        public void Play(string name)
        {
            Sound sound = null;

            //�Ű����� �̸��� ���� Ŭ�� ã��
            foreach (var s in sounds)
            {
                if (s.name == name)
                {
                    sound = s;
                    break;
                }
            }
            //�Ű����� �̸��� ���� Ŭ���� ������

            if (sound == null)
            {
                Debug.Log($"Cannot find + {name}");
                return;
            }
            sound.source.Play();
        }
        public void Stop(string name)
        {
            Sound sound = null;

            //�Ű����� �̸��� ���� Ŭ�� ã��
            foreach (var s in sounds)
            {
                if (s.name == name)
                {
                    sound = s;
                    if (s.name == bgmSound)
                    {
                        bgmSound = "";
                    }
                    break;
                }
            }
            //�Ű����� �̸��� ���� Ŭ���� ������

            if (sound == null)
            {
                Debug.Log($"Cannot find + {name}");
                return;
            }
            sound.source.Stop();
        }


        //����� ���
        public void PlayBgm(string name)
        {
            //����� �̸� üũ
            if (bgmSound == name)
            {
                return;
            }
            //����� ����
            StopBgm();

            Sound sound = null;
            foreach (var s in sounds)
            {
                if (s.name == name)
                {

                    bgmSound = name;
                    sound = s;
                    break;
                }
            }
            //�Ű����� �̸��� ���� Ŭ���� ������
            if (sound == null)
            {
                Debug.Log($"Cannot Find {name}");
                return;
            }

            sound.source.Play();
        }

        public void StopBgm()
        {
            Stop(bgmSound);
        }

    }


}


