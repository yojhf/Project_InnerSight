using UnityEngine;

namespace InnerSight_Kys
{
    //���� ��� ����
    [System.Serializable]
    public class Sound
    {
        public string name;         //����� ���� �̸�

        public AudioClip clip;      //����� ����
        [Range(0f, 1f)]
        public float volume;        //��� �Ҹ� ũ��
        [Range(0.1f, 3f)]
        public float pitch;         //��� �ӵ�

        public bool loop;           //�ݺ� ��� ����

        [HideInInspector]
        public AudioSource source;  //������ ����� ������ҽ�
    }
}