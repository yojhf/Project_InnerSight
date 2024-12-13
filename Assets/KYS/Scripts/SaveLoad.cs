using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using InnerSight_Kys;

namespace InnerSight_Kys
{
    // ���� ������ ���� ����/�������� ���� - ����ȭ ����
    public class SaveLoad
    {
        #region Variables
        private static string fileName = "/playData.arr";
        #endregion

        public static void SaveData()
        {
            // ���� �̸�, ��� ����
            string path = Application.persistentDataPath + fileName;

            // ������ �����͸� ����ȭ �غ�
            BinaryFormatter formatter = new BinaryFormatter();

            // ���� ���� - �����ϸ� ���� ��������, �������� ������ ���� ���� ����
            FileStream fs = new FileStream(path, FileMode.Create);

            // ������ ������ ����
            PlayData playData = new PlayData();
            Debug.LogFormat($"Save sceneNumber: {playData.sceneNumber}");

            // �غ��� �����͸� ����ȭ ����
            formatter.Serialize(fs, playData);

            // ���� Ŭ����
            fs.Close();
        }

        public static PlayData LoadData()  // ��ȯ Ÿ���� PlayData�� ����
        {
            PlayData playData;

            // ���� �̸�, ��� ����
            string path = Application.persistentDataPath + fileName;

            // ������ ��ο� ����� ������ �ִ��� ������ üũ
            if (File.Exists(path))
            {
                // ������ ����
                // ������ �����͸� ����ȭ �غ�
                BinaryFormatter formatter = new BinaryFormatter();

                // ���� ���� - �����ϸ� ���� ��������, �������� ������ ���� ���� ����
                FileStream fs = new FileStream(path, FileMode.Open);

                // ���Ͽ� ����ȭ�� ����� �����͸� ������ȭ�ؼ� �����´�
                playData = formatter.Deserialize(fs) as PlayData;
                Debug.LogFormat($"Load sceneNumber: {playData.sceneNumber}");

                // ���� Ŭ����
                fs.Close();
            }
            else
            {
                // ������ ����
                Debug.Log("Not Found Load File");
                playData = null;
            }

            return playData;
        }

        public static void DeleteFile()
        {
            string path = Application.persistentDataPath + fileName;
            File.Delete(path);
        }
    }
}
