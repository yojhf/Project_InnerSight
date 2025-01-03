using InnerSight_Seti;
using MyVRSample;
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
            isReset = true;
            InGameUIManager.instance.InventoryManager.ShowItem(false);
            Time.timeScale = timeScale;
        }

        public void ResetPause()
        {
            isReset = false;
            Time.timeScale = 1.0f;
        }

        IEnumerator StartReset()
        {
            // 리셋 순서
            // => 시간 멈춤 -> 정산 UI 켬 -> 정산 UI 끔 -> fadeout -> fadein -> 시간 정상화 -> 가상시간 리셋 -> 플레이 
            Pause();

            npc_Manager.enabled = false;

            inGameUI_DayCycle.DayResetUI();

            yield return new WaitForSecondsRealtime(inGameUI_DayCycle.CoolTime);

            SceneFade.instance.FadeOut(null);

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
            // 엘릭서 가격 랜덤 초기화
            Cost_Random.Instance.RandomPrice();
            // NPC 난이도 증가
            NPCGenManager.Instance.NPCGenTimeUp();
            // 금액 0원 이하 시 게임오버
            GameOverManager.Instance.CurrentGoldCheck();
        }
    }
}