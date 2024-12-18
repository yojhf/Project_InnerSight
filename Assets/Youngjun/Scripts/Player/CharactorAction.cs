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

        InventoryManager inventoryManager;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            playerSetting = GetComponent<PlayerSetting>();
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

            // ��ư ��ȣ�ۿ�
            LeftYButtonInputDown();
            RightBButtonInputDown();
            RightXInputDown();

        }

        void OnStorage()
        {
            if (InputActManager.Instance.IsStorage() && InputActManager.Instance.IsRightSelect())
            {
                playerSetting.PlayerInteraction.ThisIsMine();
            }
        }

        void RightXInputDown()
        {
            if (InputActManager.Instance.IsStorage())
            {
                if (playerSetting.Merchant != null)
                {
                    // ���ΰ��� �ŷ� ����
                    playerSetting.Merchant.Interaction();
                }
            }
        }

        void LeftAct()
        {
            if (InputActManager.Instance.IsLeftAct())
            {
                
            }
            else 
            {
      
            }
        }

        void LeftSelect()
        {
            if (InputActManager.Instance.IsLeftSelect())
            {

            }

        }

        void RightAct()
        {
            if (InputActManager.Instance.IsRightAct())
            {

            }
        }
        void RightSelect()
        {
            if (InputActManager.Instance.IsRightSelect())
            {

                
            }
        }

        void LeftStorageAct()
        {
            if (InputActManager.Instance.IsLeftStorage())
            {
                // ������Ʈ �ʵ�� ����
                GetBackStoeage();
            }
            if (InputActManager.Instance.IsLeftStorageRl())
            {
                // �κ��丮���� ������Ʈ ���� �� ������ ����
                inventoryManager.ResetData();

            }
        }

        // ���� Select ButtonDown
        void LeftSelectInputDown()
        {
            if (InputActManager.Instance.IsLeftSelectPress())
            {
                IsGrap = true;
            }
        }

        // ������ Select ButtonDown
        void RightSelectInputDown()
        {
            if (InputActManager.Instance.IsRightSelectPress())
            {
                IsGrap = true;
            }
        }

        // ���� Select ButtonUp
        void LeftSelectInputUp()
        {
            if (InputActManager.Instance.IsLeftSelectReleased())
            {
                IsGrap = false;
            }
        }

        // ������ Select ButtonUp
        void RightSelectInputUp()
        {
            if (InputActManager.Instance.IsRightSelectReleased())
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
        
        // ���� Y ��ư
        void LeftYButtonInputDown()
        {
            if (InputActManager.Instance.IsLeftYButtonDown())
            {
                if (playerSetting.Merchant is NPC_Merchant_Elixir)
                {
                    NPC_Merchant_Elixir merchant = playerSetting.Merchant as NPC_Merchant_Elixir;
                    ElixirShopManager elixirShopManager = merchant.shopManager as ElixirShopManager;
                    if (elixirShopManager.OnTrade)
                    {
                        Debug.Log("Y");
                        // ���� �ŷ� �� ���� ������
                        elixirShopManager.CountDown();
                    }
                }
            }
        }
        void RightBButtonInputDown()
        {
            if (InputActManager.Instance.IsRightBButtonDown())
            {
                if (playerSetting.Merchant is NPC_Merchant_Elixir)
                {
                    NPC_Merchant_Elixir merchant = playerSetting.Merchant as NPC_Merchant_Elixir;
                    ElixirShopManager elixirShopManager = merchant.shopManager as ElixirShopManager;
                    if (elixirShopManager.OnTrade)
                    {
                        Debug.Log("Y");
                        // ���� �ŷ� �� ���� �ø���
                        elixirShopManager.CountUp();
                    }
                }
            }
        }
    }
}

