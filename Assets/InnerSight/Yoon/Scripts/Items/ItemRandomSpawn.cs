using InnerSight_Seti;
using System.Collections;
using UnityEngine;

namespace InnerSight_Yoon
{

    public class ItemRandomSpawn : MonoBehaviour
    {
        // 소환할 Object
        public RespawnRange respawnRange; // RespawnRange 참조
        public ItemDatabase itemDatabase; // ItemDatabase 참조

        private void Start()
        {
            StartCoroutine(RandomRespawn_Coroutine());
        }

        IEnumerator RandomRespawn_Coroutine()
        {
            while (true)
            {
                // 랜덤 대기 시간
                yield return new WaitForSeconds(Random.Range(1f, 5f));

                // 랜덤 아이템 선택
                int randomIndex = Random.Range(0, itemDatabase.itemList.Count);
                ItemKey randomItem = itemDatabase.itemList[randomIndex];

                // 랜덤 위치 가져오기
                Vector3 spawnPosition = respawnRange.RandomPosition();

                // 아이템 프리팹 생성
                Instantiate(randomItem.itemPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }

}