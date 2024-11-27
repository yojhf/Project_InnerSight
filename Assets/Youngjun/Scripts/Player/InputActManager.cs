using MyFPS;
using UnityEngine.InputSystem;
using UnityEngine;
using InnerSight_Seti;

public class InputActManager : MonoBehaviour
{
    public static InputActManager Instance;

    public InputActionProperty leftAction;
    public InputActionProperty rightAction;
    public InputActionProperty leftSelect;
    public InputActionProperty rightSelect;
    public InputActionProperty storage;

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

    public bool IsStorage()
    {
        bool _storage = storage.action.WasPressedThisFrame();

        return _storage;
    }




}
