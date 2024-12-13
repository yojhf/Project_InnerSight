using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using InnerSight_Kys;

namespace InnerSight_Kys
{
    // 게임 데이터 파일 저장/가져오기 구현 - 이진화 저장
    public class SaveLoad
    {
        #region Variables
        private static string fileName = "/playData.arr";
        #endregion

        public static void SaveData()
        {
            // 파일 이름, 경로 지정
            string path = Application.persistentDataPath + fileName;

            // 저장할 데이터를 이진화 준비
            BinaryFormatter formatter = new BinaryFormatter();

            // 파일 접근 - 존재하면 파일 가져오기, 존재하지 않으면 파일 새로 생성
            FileStream fs = new FileStream(path, FileMode.Create);

            // 저장할 데이터 셋팅
            PlayData playData = new PlayData();
            Debug.LogFormat($"Save sceneNumber: {playData.sceneNumber}");

            // 준비한 데이터를 이진화 저장
            formatter.Serialize(fs, playData);

            // 파일 클로즈
            fs.Close();
        }

        public static PlayData LoadData()  // 반환 타입을 PlayData로 변경
        {
            PlayData playData;

            // 파일 이름, 경로 지정
            string path = Application.persistentDataPath + fileName;

            // 지정된 경로에 저장된 파일이 있는지 없는지 체크
            if (File.Exists(path))
            {
                // 파일이 있음
                // 가져올 데이터를 이진화 준비
                BinaryFormatter formatter = new BinaryFormatter();

                // 파일 접근 - 존재하면 파일 가져오기, 존재하지 않으면 파일 새로 생성
                FileStream fs = new FileStream(path, FileMode.Open);

                // 파일에 이진화로 저장된 데이터를 역이진화해서 가져온다
                playData = formatter.Deserialize(fs) as PlayData;
                Debug.LogFormat($"Load sceneNumber: {playData.sceneNumber}");

                // 파일 클로즈
                fs.Close();
            }
            else
            {
                // 파일이 없음
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
