using System.Collections;
using UnityEngine;
using TMPro;
using Unity.XR.CoreUtils;

namespace InnerSight_Seti
{
    // �÷��̾��� ��縦 ��ǳ������ ����ϴ� UI
    public class DialogueUI : MonoBehaviour
    {
        // �ʵ�
        #region Variables
        private PlayerSetting player;

        // UI ���� �ʵ�
        private IEnumerator beforeDialogueCor;
        private RectTransform dialogueScaler;
        [SerializeField]
        private TextMeshProUGUI dialogueText;

        // ��ǳ�� ũ�� ����
        private float scaleFactor = 0;
        private float scaleSharpness = 7.5f;
        private Vector3 initialScale;
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        private void Start()
        {
            player = GetComponentInParent<UIManager>().Player;

            dialogueScaler = this.transform.Find("DialogueScaler").GetComponent<RectTransform>();
            //dialogueText = GetComponentInChildren<TextMeshProUGUI>();

            initialScale = dialogueScaler.localScale;
            dialogueScaler.localScale = Vector3.zero;

            //player.SetDialogueUI(this);
        }
        #endregion

        // �޼���
        #region Methods
        public void Dialogue(string talk, float delay = 5)
        {
            StopDialogue();
            beforeDialogueCor = Talk(talk, delay);
            StartCoroutine(beforeDialogueCor);
        }

        public void StopDialogue()
        {
            if (beforeDialogueCor != null)
            {
                StopCoroutine(beforeDialogueCor);
                beforeDialogueCor = null;
            }
        }
        #endregion

        // ��Ÿ ��ƿ��Ƽ
        #region Utilities
        IEnumerator Talk(string talk, float delay)
        {
            dialogueText.text = talk;

            // OpenRoutine
            dialogueScaler.gameObject.SetActive(true);
            while (scaleFactor < 0.95f)
            {
                scaleFactor += scaleSharpness * Time.deltaTime;
                dialogueScaler.localScale = initialScale * scaleFactor;
                yield return null;
            }
            dialogueScaler.localScale = initialScale;
            // OpenRoutine ����

            yield return new WaitForSeconds(delay);

            // CloseRoutine
            while (scaleFactor > 0.05f)
            {
                scaleFactor -= scaleSharpness * Time.deltaTime;
                dialogueScaler.localScale = initialScale * scaleFactor;
                yield return null;
            }
            dialogueScaler.localScale = Vector3.zero;
            dialogueScaler.gameObject.SetActive(false);
            // CloseRoutine ����

            yield break;
        }
        #endregion
    }
}