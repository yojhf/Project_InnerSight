using System.Collections.Generic;
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
    public class Codex_Recipe_Manager : MonoBehaviour
    {
        // 필드
        #region Variables
        // 단순 필드
        private int firstElement = 0;
        private bool isFirstElement = false;
        private const int identifier = 2000;    // (itemID - identifier >= 0)인 아이템만 읽는다

        // 도감-레시피 UI
        private List<Image> unIdentified = new();

        // 클래스
        private PlayerInteraction player;
        [SerializeField]
        private ItemDatabase itemDatabase;
        #endregion

        // 속성
        public Dictionary<ItemKey, ItemValueRecipe> CodexRecipe { get; private set; } = new();

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            player = FindFirstObjectByType<PlayerInteraction>();
            player.SetCodex(this);

            unIdentified.AddRange(transform.GetChild(0).GetChild(0).GetChild(2).GetComponentsInChildren<Image>());
            unIdentified.AddRange(transform.GetChild(0).GetChild(0).GetChild(3).GetComponentsInChildren<Image>());

            Initialize();
        }
        #endregion

        // 메서드
        #region Methods
        // 아이템을 획득할 때마다 확인
        public void IdentifyRecipe(ItemKey itemKey)
        {
            ItemKey elementOrElixir = CollectionUtility.FirstOrNull(CodexRecipe.Keys, key => key.itemID == itemKey.itemID);
            if (elementOrElixir != null)
            {
                CodexRecipe[elementOrElixir].codexDefine = true;

                int i = CodexRecipe[elementOrElixir].codexIndex;
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
        #endregion
    }
}