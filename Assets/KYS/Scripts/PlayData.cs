using InnerSight_Seti;
using UnityEngine;

namespace InnerSight_Kys
{
    //���Ͽ� ������ ���� �÷��� ������ ���
    [System.Serializable]
    public class PlayData
    {
        public int sceneNumber;
        public int ammoCount;
        public bool hasGun;

        //... health

        //������
        public PlayData()
        {
            sceneNumber = PlayerStats.Instance.SceneNumber;
            ammoCount = PlayerStats.Instance.AmmoCount;
            hasGun = PlayerStats.Instance.HasGun;
        }
    }
}
