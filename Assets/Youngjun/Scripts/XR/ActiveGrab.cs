using InnerSight_Seti;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ActiveGrab : MonoBehaviour
{
    public InventoryManager inventoryManager;

    public GameObject leftRay;
    public GameObject rightRay;

    public XRRayInteractor leftGrab;
    public XRRayInteractor rightGrab;

    // Update is called once per frame
    void Update()
    {
        ShowRay();
    }

    void ShowRay()
    {
        bool isLeftHover = leftGrab.TryGetHitInfo(out Vector3 leftPos, out Vector3 leftNomal, out int leftNum, out bool leftValid);
        bool isRightHover = rightGrab.TryGetHitInfo(out Vector3 rightPos, out Vector3 rightNomal, out int rightNum, out bool rightValid);


        if (InputActManager.Instance.IsLeftAct() /*&& !isLeftHover*/)
        {
            leftRay.SetActive(true);
        }
        else
        {
            if (!inventoryManager.IsOpenInventory)
            {
                leftRay.SetActive(false);
            }


        }
        if (InputActManager.Instance.IsRightAct()/* && !isRightHover*/)
        {
            rightRay.SetActive(true);
        }
        else
        {
            if (!inventoryManager.IsOpenInventory)
            {
                rightRay.SetActive(false);
            }
        }

    }
}

