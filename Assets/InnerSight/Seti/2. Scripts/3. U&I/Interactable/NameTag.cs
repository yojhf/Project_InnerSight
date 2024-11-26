using TMPro;

namespace InnerSight_Seti
{
    // 네임 태그
    public class NameTag : Interactive
    {
        // 필드
        #region Variables
        protected string itemName = "";
        protected TextMeshProUGUI interactiveText;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        protected override void Start()
        {
            base.Start();

            interactiveText = GetComponentInChildren<TextMeshProUGUI>();

            interactiveText.text = itemName;
        }
        #endregion

        // 메서드
        #region Methods
        public void DefineName(string itemName)
        {
            this.itemName = itemName;
        }
        #endregion
    }
}