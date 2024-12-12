using UnityEngine;
using TMPro;

namespace InnerSight_Seti
{
    /// <summary>
    /// ���� NPC ���� UI
    /// </summary>
    public class ShopUI : MonoBehaviour
    {
        // �ʵ�
        #region Variables
        private GameObject shopUI;
        private TextMeshProUGUI goldText;
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        private void Start()
        {
            shopUI = transform.GetChild(0).GetChild(0).gameObject;
            goldText = shopUI.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        }
        #endregion

        // �޼���
        #region Methods
        // ���� UI ����ġ
        public void SwitchUI(bool isOpen)
        {
            shopUI.SetActive(isOpen);
            goldText.text = PlayerStats.Instance.CurrentGold.ToString() + " G";
        }
        #endregion
    }
}