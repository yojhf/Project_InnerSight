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
        private bool isGrap = false;

        public bool IsGrap 
        {
            get { return isGrap; }
            set { isGrap = value; }

        }

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
            // Button ������Ʈ 
            OnStorage();
            LeftAct();
            LeftSelect();
            RightAct();
            RightSelect();

            // Active Button �ѹ��� ����(����)
            LeftStorageAct();

            // Select Button �ѹ��� ����
            LeftSelectInputDown();
            RightSelectInputDown();
            LeftSelectInputUp();
            RightSelectInputUp();

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
                // ������Ʈ �ʵ�� ����
                GetBackStoeage();
            }
            if (inputActManager.IsLeftStorageRl())
            {
                // �κ��丮���� ������Ʈ ���� �� ������ ����
                inventoryManager.ResetData();

            }
        }

        // ���� Select ButtonDown
        void LeftSelectInputDown()
        {
            if (inputActManager.IsLeftSelectPress())
            {
                IsGrap = true;
            }
        }

        // ������ Select ButtonDown
        void RightSelectInputDown()
        {
            if (inputActManager.IsRightSelectPress())
            {
                IsGrap = true;
            }
        }

        // ���� Select ButtonUp
        void LeftSelectInputUp()
        {
            if (inputActManager.IsLeftSelectReleased())
            {
                IsGrap = false;
            }
        }

        // ������ Select ButtonUp
        void RightSelectInputUp()
        {
            if (inputActManager.IsRightSelectReleased())
            {
                IsGrap = false;
            }
        }

        #region ��ȣ�ۿ�
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

