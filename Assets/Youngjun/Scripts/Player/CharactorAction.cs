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

            LeftStorageAct();
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
                //GetBackStoeage();
            }
            else 
            {
      
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

        void LeftStorageAct()
        {
            if (inputActManager.IsLeftStorage())
            {
                // 오브젝트 필드로 꺼냄
                GetBackStoeage();
            }
            if (inputActManager.IsLeftStorageRl())
            {
                // 인벤토리에서 오브젝트 꺼낸 후 데이터 리셋
                inventoryManager.ResetData();

            }
        }

        void LeftSelectInput()
        {
            if (inputActManager.IsLeftSelectReleased())
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

