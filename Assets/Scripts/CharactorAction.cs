using UnityEngine;
using UnityEngine.InputSystem;

public class CharactorAction : MonoBehaviour
{
    InputActManager inputActManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputActManager = GetComponent<InputActManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Onjump();
        LeftAct();
        LeftSelect();
        RightAct();
        RightSelect();
    }


    void Onjump()
    {
        if (inputActManager.IsJump())
        {
            Debug.Log(inputActManager.IsJump());
            //rigid.AddForce(Vector3.up * jump_power);
        }
    }

    void LeftAct()
    {
        if (inputActManager.IsLeftAct())
        {
            Debug.Log("LeftAct");
        }
    }

    void LeftSelect()
    {
        if (inputActManager.IsLeftSelect())
        {
            Debug.Log("LeftSelect");
        }

    }

    void RightAct()
    {
        if (inputActManager.IsRightAct())
        {
            Debug.Log("RightAct");
        }
    }
    void RightSelect()
    {
        if (inputActManager.IsRightSelect())
        {
            Debug.Log("RightSelect");
        }
    }
}
