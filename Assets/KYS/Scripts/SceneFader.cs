using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace InnerSight_Kys
{
    public class SceneFader : MonoBehaviour
    {
        #region Variables
        //Fader �̹���
        public Image image;
        public AnimationCurve curve;
        #endregion

        private void Start()
        {
            //�ʱ�ȭ: ���۽� ȭ���� ���������� ����
            image.color = new Color(0f, 0f, 0f, 1f);
        }

        public void FromFade(float delayTime = 0f)
        {
            //�� ���۽� ���̵��� ȿ��
            StartCoroutine(FadeIn(delayTime));
        }

        IEnumerator FadeIn(float delayTime)
        {
            if (delayTime > 0f)
            {
                yield return new WaitForSeconds(delayTime);
            }

            //1�ʵ��� image a 1-> 0
            float t = 1f;

            while (t > 0)
            {
                t -= Time.deltaTime;
                float a = curve.Evaluate(t);
                image.color = new Color(0f, 0f, 0f, a);
                yield return 0f;
            }
        }

        public void FadeTo(string sceneName)
        {
            StartCoroutine(FadeOut(sceneName));
        }

        public void FadeTo(int sceneNumber)
        {
            StartCoroutine(FadeOut(sceneNumber));
        }

        IEnumerator FadeOut(string sceneName)
        {
            //1�ʵ��� image a 0-> 1
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime;
                float a = curve.Evaluate(t);
                image.color = new Color(0f, 0f, 0f, a);
                yield return 0f;
            }

            //������ �ε�
            SceneManager.LoadScene(sceneName);
        }

        IEnumerator FadeOut(int sceneNumber)
        {
            //1�ʵ��� image a 0-> 1
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime;
                float a = curve.Evaluate(t);
                image.color = new Color(0f, 0f, 0f, a);
                yield return 0f;
            }

            //������ �ε�
            SceneManager.LoadScene(sceneNumber);
        }

    }
}