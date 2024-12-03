using UnityEngine;

namespace InnerSight_Yoon
{

    public class RespawnRange : MonoBehaviour
    {
        // RespawnRange 오브젝트
        public GameObject rangeObject;
        private BoxCollider rangeCollider;

        private void Awake()
        {
            rangeCollider = rangeObject.GetComponent<BoxCollider>();
        }

        public Vector3 RandomPosition()
        {


            // BoxCollider의 경계를 기준으로 랜덤 위치 계산
            Vector3 center = rangeCollider.bounds.center; // 중심
            Vector3 size = rangeCollider.bounds.size; // 크기

            float randomX = Random.Range(-size.x / 2f, size.x / 2f);
            float randomZ = Random.Range(-size.z / 2f, size.z / 2f);

            return new Vector3(center.x + randomX, center.y, center.z + randomZ);

            /*
                        Vector3 originPosition = rangeObject.transform.position;
                        // 콜라이더의 사이즈를 가져오는 bound.size 사용
                        float range_X = rangeCollider.bounds.size.x;
                        float range_Z = rangeCollider.bounds.size.z;

                        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
                        range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);
                        Vector3 RandomPostion = new Vector3(range_X, 0f, range_Z);

                        Vector3 respawnPosition = originPosition + RandomPostion;
                        return respawnPosition;
            */
        }
    }

}