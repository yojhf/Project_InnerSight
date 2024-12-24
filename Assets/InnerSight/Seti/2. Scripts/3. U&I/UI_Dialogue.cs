using System.Collections;
using UnityEngine;
using TMPro;

namespace InnerSight_Seti
{
    // 플레이어의 대사를 말풍선으로 출력하는 UI
    public class UI_Dialogue : Singleton<UI_Dialogue>
    {
        // 필드
        #region Variables
        // UI 제어 필드
        private IEnumerator beforeDialogueCor;
        private RectTransform dialogueScaler;
        [SerializeField]
        private TextMeshProUGUI dialogueText;

        // 말풍선 크기 조절
        private float scaleFactor = 0;
        private float scaleSharpness = 7.5f;
        private Vector3 initialScale;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            dialogueScaler = this.transform.Find("DialogueScaler").GetComponent<RectTransform>();
            //dialogueText = GetComponentInChildren<TextMeshProUGUI>();

            initialScale = dialogueScaler.localScale;
            dialogueScaler.localScale = Vector3.zero;
        }

        private void LateUpdate()
        {
            transform.LookAt(Camera.main.transform);
        }
        #endregion

        // 메서드
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

        // 기타 유틸리티
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
            // OpenRoutine 종료

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
            // CloseRoutine 종료

            yield break;
        }
        #endregion
    }
}