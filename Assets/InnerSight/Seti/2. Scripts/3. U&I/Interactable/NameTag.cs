using TMPro;

namespace InnerSight_Seti
{
    // ���� �±�
    public class NameTag : Interactive
    {
        // �ʵ�
        #region Variables
        protected string itemName = "";
        protected TextMeshProUGUI interactiveText;
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        protected override void Start()
        {
            base.Start();

            interactiveText = GetComponentInChildren<TextMeshProUGUI>();

            interactiveText.text = itemName;
        }
        #endregion

        // �޼���
        #region Methods
        public void DefineName(string itemName)
        {
            this.itemName = itemName;
        }
        #endregion
    }
}