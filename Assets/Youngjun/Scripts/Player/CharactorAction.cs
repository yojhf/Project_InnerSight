using InnerSight_Seti;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Noah
{
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
}

