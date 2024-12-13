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
    public InputActionProperty yBtn;
    public InputActionProperty bBtn;

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
        float L_Select = leftSelect.action.ReadValue<float>();

        return L_Select > 0.1f;
    }
    public bool IsRightSelect()
    {
        float R_Select = rightSelect.action.ReadValue<float>();

        return R_Select > 0.1f;
    }

    public bool IsStorage()
    {
        bool _storage = storage.action.WasPressedThisFrame();

        return _storage;
    }

    public bool IsLeftStorage()
    {
        bool left_storage = leftAction.action.WasPressedThisFrame();

        return left_storage;
    }

    public bool IsLeftStorageRl()
    {
        bool left_storage = leftAction.action.WasReleasedThisFrame();

        return left_storage;
    }

    public bool IsRightStorage()
    {
        bool right_storage = rightAction.action.WasPressedThisFrame();

        return right_storage;
    }

    public bool IsRightStorageRl()
    {
        bool right_storage = rightAction.action.WasReleasedThisFrame();

        return right_storage;
    }

    public bool IsLeftSelectPress()
    {
        bool leftPr = leftSelect.action.WasPressedThisFrame();

        return leftPr;
    }

    public bool IsLeftSelectReleased()
    {
        bool leftRl = leftSelect.action.WasReleasedThisFrame();

        return leftRl;
    }

    public bool IsRightSelectPress()
    {
        bool rightPr = rightSelect.action.WasPressedThisFrame();

        return rightPr;
    }

    public bool IsRightSelectReleased()
    {
        bool rightRl = rightSelect.action.WasReleasedThisFrame();

        return rightRl;
    }

    public bool IsLeftYButtonDown()
    {
        bool isYBtn = yBtn.action.WasPressedThisFrame();

        return isYBtn;
    }

    public bool IsLeftYButtonUp()
    {
        bool isYBtn = yBtn.action.WasReleasedThisFrame();

        return isYBtn;
    }

    public bool IsRightBButtonDown()
    {
        bool isBBtn = bBtn.action.WasPressedThisFrame();

        return isBBtn;
    }

    public bool IsRightBButtonUp()
    {
        bool isBBtn = bBtn.action.WasReleasedThisFrame();

        return isBBtn;
    }

}
