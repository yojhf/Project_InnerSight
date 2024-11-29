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
                playerSetting.PlayerInteraction.ThisIsMine();
            }
        }

        void LeftAct()
        {
            if (inputActManager.IsLeftAct())
            {
                GetBackStoeage();
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

            }

        }

        void RightAct()
        {
            if (inputActManager.IsRightAct())
            {

            }
        }
        void RightSelect()
        {
            if (inputActManager.IsRightSelect())
            {

                
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

