using UnityEngine;

public class MainControl : MonoBehaviour
{    
    // Update is called once per frame
    void Update()
    {
        //JoysitckUp();
        JoysitckDown();
    }

    void JoysitckUp()
    {
        if (InputActManager.Instance.JoystickButtonUp())
        {
            Debug.Log("up");
        }
    }

    void JoysitckDown()
    {
        if (InputActManager.Instance.JoystickButtonDown())
        {
            Debug.Log("down");
        }
    }
}
