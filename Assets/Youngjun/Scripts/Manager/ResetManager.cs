using InnerSight_Seti;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Noah
{
    public class ResetManager : Singleton<ResetManager>
    {
        private Transform player;

        // Instance
        [SerializeField] private InGameUI_DayCycle inGameUI_DayCycle;
        [SerializeField] private NPC_Manager npc_Manager;
        [SerializeField] private NPCCount npcCount;
        [SerializeField] private DayOfTime dayOfTime;

        private float timeScale = 0;

        private bool isReset = false;
        public bool IsReset => isReset;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            player = FindAnyObjectByType<PlayerSetting>().transform;
        }

        public void DailyReset()
        {
            if (!isReset)
            {
                StartCoroutine(StartReset());
            }
        }
        public void Pause()
        {
            Time.timeScale = timeScale;
        }

        public void ResetPause()
        {
            Time.timeScale = 1.0f;
        }

        IEnumerator StartReset()
        {
            isReset = true;

            // 리셋 순서
            // => 시간 멈춤 -> 정산 UI 켬 -> 정산 UI 끔 -> fadeout -> fadein -> 시간 정상화 -> 가상시간 리셋 -> 플레이 
            Pause();

            npc_Manager.enabled = false;

            inGameUI_DayCycle.DayResetUI();

            yield return new WaitForSecondsRealtime(5f);

            SceneFade.instance.FadeOut(null);

            Debug.Log("ASD");

            yield return new WaitForSecondsRealtime(1f);

            SceneFade.instance.FadeIn(null);

            ResetPause();

            ResetData();
        }

        void ResetData()
        {
            // 리셋 해야될 시스템
            // 날짜 업데이트
            dayOfTime.CheckDayTransition();
            // 쓰레기 오브젝트 리스폰
            SpwanManager.Instance.SpwanCon();
            // 월세 증가
            PlayerCostManager.Instance.UpdateShopTax();
            // 플레이어 위치 초기화
            player.position = player.GetComponent<PlayerSetting>().StartPos;
            // NPC 초기화
            npc_Manager.enabled = true;
            // NPCCount 초기화
            npcCount.ResetNPCCount();

            isReset = false;
        }
    }
}