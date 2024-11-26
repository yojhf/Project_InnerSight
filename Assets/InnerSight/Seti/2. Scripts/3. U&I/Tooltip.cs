using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace InnerSight_Seti
{
    /// <summary>
    /// 꼬리표 UI의 기능을 관리하는 클래스
    /// </summary>
    public class Tooltip : MonoBehaviour
    {
        // 필드
        #region Variables
        // 전용 변수
        private GameObject function;
        private Image background;
        private TextMeshProUGUI nameText;
        private TextMeshProUGUI figureText;
        private IEnumerator functionCor;

        // 초기 판정
        private bool isFirst = true;

        // 스케일 기반 페이드
        private float scaleFactor;
        private Vector3 initialScale;

        // 알파값 기반 페이드
        private float currentAlpha;
        private float initialAlpha;
        private float initialTextAlpha;
        [SerializeField]
        private float fadeSharpness = 10f;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            function = transform.GetChild(0).gameObject;
            background = function.transform.GetChild(0).GetComponent<Image>();
            nameText = function.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            figureText = function.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

            initialScale = function.transform.localScale;
            initialAlpha = background.color.a;
            initialTextAlpha = nameText.color.a;
            currentAlpha = 0f;
        }
        #endregion

        // 메서드
        #region Methods
        public void SetFunction(string name, float currentFigure, float maxFigure)
        {
            nameText.text = name;
            figureText.text = $"{Mathf.RoundToInt(currentFigure)}/{maxFigure}";
        }

        public void ShowFunction()
        {
            if (function.activeSelf) function.SetActive(false);
            if (functionCor != null) StopCoroutine(functionCor);

            if (isFirst) functionCor = AppearFunction();
            else functionCor = FadeFunction();

            function.SetActive(true);
            StartCoroutine(functionCor);
        }
        #endregion

        // 기타 유틸리티
        #region Utilities
        IEnumerator AppearFunction()
        {
            isFirst = false;
            scaleFactor = 0f;

            // OpenRoutine
            while (scaleFactor < 0.95f)
            {
                scaleFactor += 5 * Time.deltaTime;
                function.transform.localScale = initialScale * scaleFactor;
                yield return null;
            }
            function.transform.localScale = initialScale;
            // OpenRoutine 종료

            yield return new WaitForSeconds(3);

            // CloseRoutine
            while (scaleFactor > 0.05f)
            {
                scaleFactor -= 5 * Time.deltaTime;
                function.transform.localScale = initialScale * scaleFactor;
                yield return null;
            }
            function.gameObject.SetActive(false);
            function.transform.localScale = initialScale;
            // CloseRoutine 종료

            functionCor = null;
            yield break;
        }

        IEnumerator FadeFunction()
        {
            // OpenRoutine
            while (currentAlpha < 0.39f)
            {
                currentAlpha = Mathf.Lerp(currentAlpha, initialAlpha, fadeSharpness * Time.deltaTime);
                ColorUtility.SetAlpha(background, currentAlpha);
                ColorUtility.SetAlpha(nameText, currentAlpha * 2.5f);
                ColorUtility.SetAlpha(figureText, currentAlpha * 2.5f);
                yield return null;
            }
            currentAlpha = initialAlpha;
            ColorUtility.SetAlpha(background, initialAlpha);
            ColorUtility.SetAlpha(nameText, initialTextAlpha);
            ColorUtility.SetAlpha(figureText, initialTextAlpha);
            // OpenRoutine 종료

            yield return new WaitForSeconds(3);

            // CloseRoutine
            while (currentAlpha > 0.01f)
            {
                currentAlpha = Mathf.Lerp(currentAlpha, 0, fadeSharpness * Time.deltaTime);
                ColorUtility.SetAlpha(background, currentAlpha);
                ColorUtility.SetAlpha(nameText, currentAlpha * 2.5f);
                ColorUtility.SetAlpha(figureText, currentAlpha * 2.5f);
                yield return null;
            }
            currentAlpha = 0f;
            ColorUtility.SetAlpha(background, 0);
            ColorUtility.SetAlpha(nameText, 0);
            ColorUtility.SetAlpha(figureText, 0);
            // CloseRoutine 종료

            function.gameObject.SetActive(false);
            functionCor = null;
            yield break;
        }
        #endregion
    }
}