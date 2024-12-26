using InnerSight_Kys;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Noah
{
    public class MainControl : MonoBehaviour
    {
        [SerializeField] private Transform buttonPar;
        private List<Transform> buttons = new List<Transform>();
        private Button defaultBtn;
        private int index;

        private bool isSelect = false;

        private void Start()
        {
            Init();
        }

        // Update is called once per frame
        void Update()
        {
            SelectMenu();
        }

        IEnumerator ButtonSelect()
        {
            while (true)
            {
                JoysitckUp();
                JoysitckDown();

                yield return new WaitForSeconds(0.15f);
            }
        }


        void Init()
        {
            for (int i = 0; i < buttonPar.childCount; i++)
            {
                buttons.Add(buttonPar.GetChild(i));
            }

            index = 0;

            defaultBtn = buttons[0].GetComponent<Button>();

            EventSystem.current.SetSelectedGameObject(defaultBtn.gameObject);

            StartCoroutine(ButtonSelect());

            AudioManager.Instance.PlayBgm("MapBgm");
        }

        void JoysitckUp()
        {
            if (InputActManager.Instance.JoystickButtonUp())
            {
                AudioManager.Instance.Play("Notification");

                isSelect = true;

                index--;

                if (index < 0)
                {
                    index = buttons.Count - 1;

                    EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);
                }
                else
                {

                    EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);
                }
            }
        }

        void JoysitckDown()
        {
            if (InputActManager.Instance.JoystickButtonDown())
            {
                AudioManager.Instance.Play("Notification");

                isSelect = true;

                index++;

                if (index >= buttons.Count)
                {
                    index = 0;

                    EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);
                }
                else
                {

                    EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);
                }


            }
        }

        void SelectMenu()
        {
            if (InputActManager.Instance.IsStorage())
            {
                if (buttons[index].GetComponent<Button>() != null)
                {
                    AudioManager.Instance.Play("BtnClick");

                    buttons[index].GetComponent<Button>().onClick.Invoke();
                }
            }
        }

        public void Tutorial()
        {
            SceneFade.instance.FadeOut("TutorialScene");
        }
        public void NewGame()
        {
            SceneFade.instance.FadeOut("PlayScene");
        }
        public void TestMode()
        {
            SceneFade.instance.FadeOut("TestScene");
            Debug.Log("Å©·¹µ÷");
        }
        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

}
