using Noah;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

            yield return new WaitForSeconds(0.2f);

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
    }

    void JoysitckUp()
    {
        if (InputActManager.Instance.JoystickButtonUp())
        {
            isSelect = true;

            Debug.Log("down");

            index--;

            Debug.Log(index);

            if (index < 0)
            {
                index = buttons.Count - 1;

                Debug.Log("down : " + index);

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
            isSelect = true;

            Debug.Log("up");

            index++;

            Debug.Log(index);
            if (index >= buttons.Count)
            {
                index = 0;
                Debug.Log("as" + index);
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
                buttons[index].GetComponent<Button>().onClick.Invoke();
            }
        }
    }

    public void NewGame()
    {
        SceneFade.instance.FadeOut("PlayScene");
    }
    public void Credit()
    {
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
