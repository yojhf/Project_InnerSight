using UnityEngine;

namespace InnerSight_Seti
{
    // 말풍선 클래스
    public class TalkBubble : Interactive
    {
        // 필드
        #region Variables
        private float distance;
        private float scaleFactor;
        private Vector3 initialScale;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        protected override void Start()
        {
            base.Start();

            initialScale = this.transform.localScale;
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();

            Resizing();
        }
        #endregion

        // 메서드
        #region Methods
        private void Resizing()
        {
            distance = Vector3.Distance(transform.position, player.transform.position);
            scaleFactor = distance / 5;
            scaleFactor = Mathf.Clamp(scaleFactor, 0.4f, 1f);
            this.transform.localScale = initialScale * scaleFactor;
        }
        #endregion
    }
}