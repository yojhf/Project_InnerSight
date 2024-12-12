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

            // ���� ����
            // => �ð� ���� -> ���� UI �� -> ���� UI �� -> fadeout -> fadein -> �ð� ����ȭ -> ����ð� ���� -> �÷��� 
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
            // ���� �ؾߵ� �ý���
            // ��¥ ������Ʈ
            dayOfTime.CheckDayTransition();
            // ������ ������Ʈ ������
            SpwanManager.Instance.SpwanCon();
            // ���� ����
            PlayerCostManager.Instance.UpdateShopTax();
            // �÷��̾� ��ġ �ʱ�ȭ
            player.position = player.GetComponent<PlayerSetting>().StartPos;
            // NPC �ʱ�ȭ
            npc_Manager.enabled = true;
            // NPCCount �ʱ�ȭ
            npcCount.ResetNPCCount();

            isReset = false;
        }
    }
}