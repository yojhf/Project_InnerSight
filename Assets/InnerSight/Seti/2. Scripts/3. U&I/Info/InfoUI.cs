using System;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace InnerSight_Seti
{
    /// <summary>
    /// 스마트폰 UI를 관리하는 클래스
    /// </summary>
    public class InfoUI : MonoBehaviour
    {
        // 필드
        #region Variables
        // 패널 갱신
        public UnityAction OnDevice;

        // 스마트폰 UI

        // 위젯 UI
        [SerializeField]
        private GameObject quickPanel;
        private GameObject widgetPanel;

        private RectTransform quickRect;
        private RectTransform widgetRect;

        // 위젯 페이드
        private TextMeshProUGUI collText;
        private TextMeshProUGUI timeText;

        // 클래스
        private Player player;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            // 참조
            player = GetComponentInParent<UIManager>().Player;
            player.SetDeviceUI(this);

            // 초기화
            widgetPanel = transform.GetChild(0).GetChild(0).gameObject;

            widgetRect = widgetPanel.GetComponent<RectTransform>();
            timeText = widgetPanel.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            collText = widgetPanel.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();

            // 대리자 등록
            OnDevice += UpdateWon;

            // 최초 갱신
            UpdateDeviceUI();
        }

        private void LateUpdate()
        {
            timeText.text = DateTime.Now.ToString("tt HH:mm");
        }
        #endregion

        // 메서드
        #region Methods
        // 갱신
        public void UpdateDeviceUI()
        {
            OnDevice?.Invoke();
        }

        private void UpdateWon()
        {
            collText.text = player.playerStates.CurrentColl.ToString() + " Gold";
        }
        #endregion
    }
}