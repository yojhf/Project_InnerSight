using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ActiveGrab : MonoBehaviour
{
    public GameObject leftRay;
    public GameObject rightRay;

    public XRRayInteractor leftGrab;
    public XRRayInteractor rightGrab;

    InputActManager inputActManager;


    private void Start()
    {
        inputActManager = GetComponent<InputActManager>();
    }
    // Update is called once per frame
    void Update()
    {
        ShowRay();
    }

    void ShowRay()
    {
        bool isLeftHover = leftGrab.TryGetHitInfo(out Vector3 leftPos, out Vector3 leftNomal, out int leftNum, out bool leftValid);
        bool isRightHover = rightGrab.TryGetHitInfo(out Vector3 rightPos, out Vector3 rightNomal, out int rightNum, out bool rightValid);


        if (inputActManager.IsLeftAct() /*&& !isLeftHover*/)
        {
            leftRay.SetActive(true);
        }
        else
        {
            leftRay.SetActive(false);
        }
        if (inputActManager.IsRightAct()/* && !isRightHover*/)
        {
            rightRay.SetActive(true);
        }
        else
        {
            rightRay.SetActive(false);
        }

    }
}

