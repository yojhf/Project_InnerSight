using InnerSight_Kys;
using InnerSight_Seti;
using MyVRSample;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Noah
{
    public class CharactorAction : MonoBehaviour
    {
        private GameObject autoGet;
        private PlayerSetting playerSetting;
        private bool isGrap = false;

        public bool IsGrap 
        {
            get { return isGrap; }
            set { isGrap = value; }

        }

        InventoryManager inventoryManager;
        GetAutoItem autoItem;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            playerSetting = GetComponent<PlayerSetting>();
            inventoryManager = transform.GetChild(0).GetChild(0).GetComponent<InventoryManager>();

            //autoGet = transform.GetChild(3).gameObject;
            //autoItem = autoGet.GetComponent<GetAutoItem>();
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
            // Active Button 한번만 반응(오른쪽)
            RightStorageAct();

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
            if (InputActManager.Instance.IsStorage() && InputActManager.Instance.IsRightSelect())
            {
                AudioManager.Instance.Play("PickUP");
                playerSetting.PlayerInteraction.ThisIsMine();
            }
        }

        void RightXInputDown()

        {
            if (InputActManager.Instance.IsStorage())
            {
                if (playerSetting.Merchant != null)
                {
                    AudioManager.Instance.Play("Throw");

                    // 상인과의 거래 시작
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
            else
            {

            }

        }

        void RightAct()
        {
            if (InputActManager.Instance.IsRightAct())
            {

            }
            else
            {

            }
        }
        void RightSelect()
        {
            if (InputActManager.Instance.IsRightSelect())
            {

            }
            else
            {

            }
        }

        void LeftStorageAct()
        {
            if (InputActManager.Instance.IsLeftStorage())
            {
                AudioManager.Instance.Play("Drop");

                // 오브젝트 필드로 꺼냄
                GetBackStoeage();
            }
            if (InputActManager.Instance.IsLeftStorageRl())
            {
                // 인벤토리에서 오브젝트 꺼낸 후 데이터 리셋
                inventoryManager.ResetData();

            }
        }

        void RightStorageAct()
        {
            if (InputActManager.Instance.IsRightStorage())
            {
                AudioManager.Instance.Play("Drop");

                // 오브젝트 필드로 꺼냄
                GetBackStoeage();
            }
            if (InputActManager.Instance.IsRightStorageRl())
            {
                // 인벤토리에서 오브젝트 꺼낸 후 데이터 리셋
                inventoryManager.ResetData();

            }
        }

        // 왼쪽 Select ButtonDown
        void LeftSelectInputDown()
        {
            if (InputActManager.Instance.IsLeftSelectPress())
            {
                IsGrap = true;
            }
        }

        // 오른쪽 Select ButtonDown
        void RightSelectInputDown()
        {
            if (InputActManager.Instance.IsRightSelectPress())
            {
                IsGrap = true;
            }
        }

        // 왼쪽 Select ButtonUp
        void LeftSelectInputUp()
        {
            if (InputActManager.Instance.IsLeftSelectReleased())
            {
                IsGrap = false;
            }
        }

        // 오른쪽 Select ButtonUp
        void RightSelectInputUp()
        {
            if (InputActManager.Instance.IsRightSelectReleased())
            {
                IsGrap = false;
            }
        }

        #region 상호작용
        void GetBackStoeage()
        {
            if (InGameUIManager.instance.inventory.activeSelf)
            {
                AudioManager.Instance.Play("Throw");

                inventoryManager.XR_WhichSelect();
            }
        }
        #endregion
        
        // 왼쪽 Y 버튼
        void LeftYButtonInputDown()
        {
            if (InputActManager.Instance.IsLeftYButtonDown())
            {
                if (playerSetting.Merchant is NPC_Merchant_Elixir)
                {
                    AudioManager.Instance.Play("BtnClick");

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
            if (InputActManager.Instance.IsRightBButtonDown())
            {
                if (playerSetting.Merchant is NPC_Merchant_Elixir)
                {
                    AudioManager.Instance.Play("BtnClick");

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

