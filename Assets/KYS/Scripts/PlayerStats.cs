using InnerSight_Kys;
using InnerSight_Seti;
using UnityEngine;

namespace InnerSight_Kys
{
    //���� ������ ȹ�� ����
    public enum PuzzleKey
    {
        ROOM01_KEY,
        LEFTEYE_KEY,
        RIGHTEYE_KET,
        MAX_KEY             //���� ������ ����
    }

    //�÷��̾��� �Ӽ�, �����Ͱ��� �����ϴ� (�̱���, DontDestory)Ŭ����.. ammoCount
    public class PlayerStats : PersistentSingleton<PlayerStats>
    {
        #region Variables
        //����� SceneNumber
        private int sceneNumber;
        public int SceneNumber
        {
            get { return sceneNumber; }
            set { sceneNumber = value; }
        }
        //���� �÷����ϰ� �ִ� SceneNumber
        private int nowSceneNumber;
        public int NowSceneNumber
        {
            get { return nowSceneNumber; }
            set { nowSceneNumber = value; }
        }

        //źȯ ����
        [SerializeField] private int ammoCount;

        public int AmmoCount
        {
            get { return ammoCount; }
            private set { ammoCount = value; }
        }

        //���� �Ҹ� ����
        private bool hasGun;

        public bool HasGun
        {
            get { return hasGun; }
            set { hasGun = value; }
        }

        //���� ���� ������ Ű
        private bool[] puzzleKeys;
        #endregion

        private void Start()
        {
            //�Ӽ���/Data �ʱ�ȭ
            puzzleKeys = new bool[(int)PuzzleKey.MAX_KEY];
        }

        public void PlayerStatInit(PlayData playData)
        {
            if (playData != null)
            {
                SceneNumber = playData.sceneNumber;
                AmmoCount = playData.ammoCount;
                hasGun = playData.hasGun;
            }
            else //����� �����Ͱ� ������
            {
                SceneNumber = 0;
                AmmoCount = 0;
                hasGun = false;
            }
        }

        public void AddAmmo(int amount)
        {
            AmmoCount += amount;
        }

        public bool UseAmmo(int amount)
        {
            //���� ���� üũ
            if (AmmoCount < amount)
            {
                Debug.Log("You need to reload!!!!");
                return false;   //��뷮���� �����ϴ�
            }

            AmmoCount -= amount;
            return true;
        }

        //���� ������ ȹ��
        public void AcquirePuzzleItem(PuzzleKey key)
        {
            puzzleKeys[(int)key] = true;
        }

        //���� �������� ���� ����
        public bool HasPuzzleItem(PuzzleKey key)
        {
            return puzzleKeys[(int)key];
        }

        //���� ȹ�� ����
        public void SetHasGun(bool value)
        {
            hasGun = value;
        }
    }
}