using UnityEngine;

namespace InnerSight_Seti
{
    /// <summary>
    /// 매일 랜덤 가격을 결정하는 클래스
    /// </summary>
    public class Cost_Random : MonoBehaviour
    {
        // 필드
        #region Variables
        // 일반 필드
        private const int identifier = 4000;    // 엘릭서만 읽는다

        // 아이템 데이터베이스
        [SerializeField]
        private ItemDatabase itemDatabase;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        #endregion

        // 메서드
        #region Methods
        // 초기화 - 아이템DB로부터 엘릭서를 읽어와 도감 딕셔너리에 저장
        /*private void Initialize()
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
        }*/
        #endregion
    }
}