using System.Collections.Generic;
using Unity.VRTemplate;
using UnityEngine;
using UnityEngine.UI;

namespace InnerSight_Seti
{
    /// <summary>
    /// 도감-조합법 UI
    /// </summary>
    /// 0. 도감용으로 사용할 아이템 리스트? 딕셔너리<아이템, 불리언>
    /// 1. 합성했을 때 아웃풋을 어떻게 인식할 것인가? 인벤토리에 넣을 때
    /// 2. 아이템DB에서 원소와 엘릭서만 읽는 방법은? ID에서 2000을 뺐을 때 양수인 아이템
    /// 3. true = 레시피, false = ???
    /// 4. UI
    public class Codex_Recipe_Manager : Singleton<Codex_Recipe_Manager>
    {
        // 필드
        #region Variables
        // 단순 필드
        private int firstElement = 0;
        private bool isFirstElement = false;
        private const int identifier = 2000;    // (itemID - identifier >= 0)인 아이템만 읽는다

        // 도감-레시피 UI
        private List<Outputs> outputs = new();
        private List<GameObject> unIdentified = new();

        // 클래스
        [SerializeField]
        private ItemDatabase itemDatabase;
        private PlayerInteraction player;
        private ElixirShopManager shopManager;
        #endregion

        // 속성
        #region Properties
        public Dictionary<ItemKey, ItemValueRecipe> CodexRecipe { get; private set; } = new();
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            player = FindFirstObjectByType<PlayerInteraction>();
            player.SetCodex(this);

            outputs.AddRange(GetComponentsInChildren<Outputs>());
            foreach (var output in outputs)
            {
                if (output != null)
                    output.GetComponent<Image>().enabled = false;
            }

            Transform undefinedElements = transform.GetChild(0).GetChild(0).GetChild(2);
            Transform undefinedElixirs = transform.GetChild(0).GetChild(0).GetChild(3);
            for (int i = 0; i < 4; i++)
            {
                unIdentified.Add(undefinedElements.GetChild(0).GetChild(i).gameObject);
            }
            for (int i = 0; i < 3; i++)
            {
                unIdentified.Add(undefinedElements.GetChild(1).GetChild(i).gameObject);
            }
            for (int i = 0; i < 4; i++)
            {
                unIdentified.Add(undefinedElixirs.GetChild(0).GetChild(i).gameObject);
            }
            for (int i = 0; i < 4; i++)
            {
                unIdentified.Add(undefinedElixirs.GetChild(1).GetChild(i).gameObject);
            }

            Initialize();
        }
        #endregion

        // 메서드
        #region Methods
        // 아이템을 획득할 때마다 확인
        public void IdentifyRecipe(ItemKey itemKey)
        {
            ItemKey elementOrElixir = CollectionUtility.FirstOrNull(CodexRecipe.Keys, key => key.itemID == itemKey.itemID);
            if (elementOrElixir != null && CodexRecipe[elementOrElixir].codexDefine == false)
            {
                CodexRecipe[elementOrElixir].codexDefine = true;
                if (itemKey.itemID > 4000)
                    shopManager.GetKnowhow(itemKey);

                int i = CodexRecipe[elementOrElixir].codexIndex;
                outputs[i].GetComponent<Image>().enabled = true;
                Destroy(unIdentified[i]);
            }
            return;
        }

        // 초기화 - 아이템DB로부터 원소와 엘릭서를 읽어와 도감 딕셔너리에 저장
        private void Initialize()
        {
            for (int i = 0; i < itemDatabase.itemList.Count; i++)
            {
                if (itemDatabase.itemList[i].itemID - identifier > 0)
                {
                    if (!isFirstElement)
                    {
                        isFirstElement = true;
                        firstElement = i;
                    }

                    ItemValueRecipe valueRecipe = new()
                    {
                        codexIndex = i - firstElement,
                        codexDefine = false
                    };
                    CodexRecipe.Add(itemDatabase.itemList[i], valueRecipe);
                }
            }
        }

        public void SetCodexToShop(ElixirShopManager shopManager)
        {
            this.shopManager = shopManager;
        }
        #endregion
    }
}