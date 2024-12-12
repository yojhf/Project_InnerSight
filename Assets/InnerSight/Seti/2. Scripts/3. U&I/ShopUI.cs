using UnityEngine;
using TMPro;

namespace InnerSight_Seti
{
    /// <summary>
    /// 상인 NPC 전용 UI
    /// </summary>
    public class ShopUI : MonoBehaviour
    {
        // 필드
        #region Variables
        private GameObject shopUI;
        private TextMeshProUGUI goldText;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            shopUI = transform.GetChild(0).GetChild(0).gameObject;
            goldText = shopUI.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        }
        #endregion

        // 메서드
        #region Methods
        // 상점 UI 스위치
        public void SwitchUI(bool isOpen)
        {
            shopUI.SetActive(isOpen);
            goldText.text = PlayerStats.Instance.CurrentGold.ToString() + " G";
        }
        #endregion
    }
}