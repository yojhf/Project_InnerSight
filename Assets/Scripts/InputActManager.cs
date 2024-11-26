using MyFPS;
using UnityEngine.InputSystem;
using UnityEngine;

public class InputActManager : MonoBehaviour
{
    public static InputActManager Instance;


    public InputActionProperty leftAction;
    public InputActionProperty rightAction;
    public InputActionProperty leftSelect;
    public InputActionProperty rightSelect;
    public InputActionProperty jump;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }


    }
    public bool IsLeftAct()
    {
        float L_act = leftAction.action.ReadValue<float>();

        return L_act > 0.1f;
    }
    public bool IsRightAct()
    {
        float R_act = rightAction.action.ReadValue<float>();

        return R_act > 0.1f;
    }
    public bool IsLeftSelect()
    {
        float R_act = leftSelect.action.ReadValue<float>();

        return R_act > 0.1f;
    }
    public bool IsRightSelect()
    {
        float R_act = rightSelect.action.ReadValue<float>();

        return R_act > 0.1f;
    }

    public bool IsJump()
    {
        bool _jump = jump.action.WasPressedThisFrame();

        return _jump;
    }


}
