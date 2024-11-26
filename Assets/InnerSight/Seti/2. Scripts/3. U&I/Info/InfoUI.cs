using System;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace InnerSight_Seti
{
    /// <summary>
    /// ����Ʈ�� UI�� �����ϴ� Ŭ����
    /// </summary>
    public class InfoUI : MonoBehaviour
    {
        // �ʵ�
        #region Variables
        // �г� ����
        public UnityAction OnDevice;

        // ����Ʈ�� UI

        // ���� UI
        [SerializeField]
        private GameObject quickPanel;
        private GameObject widgetPanel;

        private RectTransform quickRect;
        private RectTransform widgetRect;

        // ���� ���̵�
        private TextMeshProUGUI collText;
        private TextMeshProUGUI timeText;

        // Ŭ����
        private Player player;
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        private void Start()
        {
            // ����
            player = GetComponentInParent<UIManager>().Player;
            player.SetDeviceUI(this);

            // �ʱ�ȭ
            widgetPanel = transform.GetChild(0).GetChild(0).gameObject;

            widgetRect = widgetPanel.GetComponent<RectTransform>();
            timeText = widgetPanel.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            collText = widgetPanel.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();

            // �븮�� ���
            OnDevice += UpdateWon;

            // ���� ����
            UpdateDeviceUI();
        }

        private void LateUpdate()
        {
            timeText.text = DateTime.Now.ToString("tt HH:mm");
        }
        #endregion

        // �޼���
        #region Methods
        // ����
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