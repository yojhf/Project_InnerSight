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
            // Button 업데이트 
            OnStorage();
            LeftAct();
            LeftSelect();
            RightAct();
            RightSelect();

            // Active Button 한번만 반응(왼쪽)
            LeftStorageAct();

            // Select Button 한번만 반응
            LeftSelectInputDown();
            RightSelectInputDown();
            LeftSelectInputUp();
            RightSelectInputUp();

            // 버튼 상호작용
            LeftYButtonInputDown();
            RightBButtonInputDown();
            RightXInputDown();

        }

        void OnStorage()
        {
            if (inputActManager.IsStorage() && inputActManager.IsRightSelect())
            {
                playerSetting.PlayerInteraction.ThisIsMine();
            }
        }

        void RightXInputDown()
        {
            if (inputActManager.IsStorage())
            {
                if (playerSetting.Merchant != null)
                {
                    // 상인과의 거래 시작
                    playerSetting.Merchant.Interaction();
                }
            }
        }

        void LeftAct()
        {
            if (inputActManager.IsLeftAct())
            {
                
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

        // 왼쪽 Select ButtonDown
        void LeftSelectInputDown()
        {
            if (inputActManager.IsLeftSelectPress())
            {
                IsGrap = true;
            }
        }

        // 오른쪽 Select ButtonDown
        void RightSelectInputDown()
        {
            if (inputActManager.IsRightSelectPress())
            {
                IsGrap = true;
            }
        }

        // 왼쪽 Select ButtonUp
        void LeftSelectInputUp()
        {
            if (inputActManager.IsLeftSelectReleased())
            {
                IsGrap = false;
            }
        }

        // 오른쪽 Select ButtonUp
        void RightSelectInputUp()
        {
            if (inputActManager.IsRightSelectReleased())
            {
                IsGrap = false;
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
        
        // 왼쪽 Y 버튼
        void LeftYButtonInputDown()
        {
            if (inputActManager.IsLeftYButtonDown())
            {
                if (playerSetting.Merchant is NPC_Merchant_Elixir)
                {
                    NPC_Merchant_Elixir merchant = playerSetting.Merchant as NPC_Merchant_Elixir;
                    ElixirShopManager elixirShopManager = merchant.shopManager as ElixirShopManager;
                    if (elixirShopManager.OnTrade)
                    {
                        Debug.Log("Y");
                        // 복수 거래 시 수량 내리기
                        elixirShopManager.CountDown();
                    }
                }
            }
        }
        void RightBButtonInputDown()
        {
            if (inputActManager.IsRightBButtonDown())
            {
                if (playerSetting.Merchant is NPC_Merchant_Elixir)
                {
                    NPC_Merchant_Elixir merchant = playerSetting.Merchant as NPC_Merchant_Elixir;
                    ElixirShopManager elixirShopManager = merchant.shopManager as ElixirShopManager;
                    if (elixirShopManager.OnTrade)
                    {
                        Debug.Log("Y");
                        // 복수 거래 시 수량 올리기
                        elixirShopManager.CountUp();
                    }
                }
            }
        }
    }
}

