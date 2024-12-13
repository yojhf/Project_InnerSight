using InnerSight_Seti;
using UnityEngine;

namespace InnerSight_Kys
{
    //������� �����ϴ� Ŭ����
    public class AudioManager : Singleton<AudioManager>
    {
        #region Variables
        public Sound[] sounds;
        private string bgmSound = "";       //���� �÷��� �Ǵ� ����� �̸�
        public string BgmSound
        {
            get { return bgmSound; }
        }
        #endregion

        protected override void Awake()
        {
            //Singletone ������
            base.Awake();

            //AudioManager �ʱ�ȭ
            foreach (var sound in sounds)
            {
                sound.source = this.gameObject.AddComponent<AudioSource>();

                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
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
                Debug.Log($"Cannot Find {name}");
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

                    //
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
                Debug.Log($"Cannot Find {name}");
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
                    bgmSound = s.name;
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