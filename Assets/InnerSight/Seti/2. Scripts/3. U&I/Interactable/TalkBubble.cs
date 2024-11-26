using UnityEngine;

namespace InnerSight_Seti
{
    // ��ǳ�� Ŭ����
    public class TalkBubble : Interactive
    {
        // �ʵ�
        #region Variables
        private float distance;
        private float scaleFactor;
        private Vector3 initialScale;
        #endregion

        // ������ ����Ŭ
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

        // �޼���
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