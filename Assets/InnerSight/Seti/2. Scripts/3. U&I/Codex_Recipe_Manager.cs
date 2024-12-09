using System.Collections.Generic;
using UnityEngine;

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
    public class Codex_Recipe_Manager : MonoBehaviour
    {
        // 필드
        #region Variables
        // 단순 필드
        private const int identifier = 2000;    // (itemID - identifier >= 0)인 아이템만 읽는다

        // 도감-레시피 UI
        [SerializeField]
        private GameObject UI_Codex_Recipe;
        private List<GameObject> unIdentified_Elements = new();
        private List<GameObject> unIdentified_Elixirs = new();

        // 클래스
        [SerializeField]
        private ItemDatabase itemDatabase;
        private InventoryManager inventoryManager;
        #endregion

        // 속성
        public Dictionary<ItemKey, ItemValueRecipe> CodexRecipe { get; private set; } = new();

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            //unIdentified_Elements

            inventoryManager = GetComponentInParent<InventoryManager>();
            Initialize();
        }
        #endregion

        // 메서드
        #region Methods
        // 초기화 - 아이템DB로부터 원소와 엘릭서를 읽어와 도감 딕셔너리에 저장
        private void Initialize()
        {
            for (int i = 0; i < itemDatabase.itemList.Count; i++)
            {
                if (itemDatabase.itemList[i].itemID - identifier > 0)
                {
                    ItemValueRecipe valueRecipe = new()
                    {
                        codexIndex = i,
                        codexDefine = false
                    };
                    CodexRecipe.Add(itemDatabase.itemList[i], valueRecipe);
                }
            }
        }
        #endregion
    }
}