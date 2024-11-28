using InnerSight_Seti;
using MyVRSample;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Noah
{
    public class CharactorAction : MonoBehaviour
    {
        private PlayerSetting playerSetting;
        
        InputActManager inputActManager;
        InventoryManager inventoryManager;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            playerSetting = GetComponent<PlayerSetting>();
            inputActManager = GetComponent<InputActManager>();
            inventoryManager = transform.GetChild(0).GetChild(0).GetComponent<InventoryManager>();
        }

        // Update is called once per frame
        void Update()
        {
            OnStorage();
            LeftAct();
            LeftSelect();
            RightAct();
            RightSelect();
        }


        void OnStorage()
        {
            if (inputActManager.IsStorage() && inputActManager.IsRightSelect())
            {
                Debug.Log("storage");
                playerSetting.PlayerInteraction.ThisIsMine();
            }
        }

        void LeftAct()
        {
            if (inputActManager.IsLeftAct())
            {
                GetBackStoeage();
                Debug.Log("LeftAct");
            }
            else 
            {
                inventoryManager.IsSelected = false;
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



        #region 상호작용
        void GetBackStoeage()
        {
            if (InGameUIManager.instance.inventory.activeSelf)
            {
                inventoryManager.XR_WhichSelect();
            }
        }
        #endregion
    }
}

