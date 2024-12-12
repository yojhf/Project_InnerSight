using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace InnerSight_Seti
{
    /// <summary>
    /// 매일 랜덤 가격을 결정하는 클래스
    /// </summary>
    public class Cost_Random : Singleton<Cost_Random>
    {
        // 필드
        #region Variables
        // 일반 필드
        private const int identifier = 4000;    // 엘릭서만 읽는다
        private List<int> elixirsIndex = new(); // 엘릭서의 인덱스
        private List<int> elixirsPrice = new(); // 엘릭서의 원래 가격
        private List<TextMeshProUGUI> elixirsText = new();

        // 아이템 데이터베이스
        [SerializeField]
        private ItemDatabase itemDatabase;
        [SerializeField]
        private GameObject priceUI;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Start()
        {
            SetElixirs();
            elixirsText.AddRange(priceUI.GetComponentsInChildren<TextMeshProUGUI>());
            RandomPrice();
        }

        private void OnDisable()
        {
            Initialize();
        }
        #endregion

        // 메서드
        #region public
        // 아이템DB의 엘릭서 가격을 랜덤하게 변경
        public void RandomPrice()
        {
            Initialize();
            for (int i = 0; i < elixirsPrice.Count; i++)
            {
                int deviation = Random.Range(-20, 21);
                itemDatabase.itemList[i + elixirsIndex[0]].itemPrice += deviation;
                elixirsText[i].text = itemDatabase.itemList[i + elixirsIndex[0]].itemPrice.ToString() + "G";
            }
        }
        #endregion
        #region private
        // 디폴트 가격으로 초기화
        private void Initialize()
        {
            for (int i = 0; i < elixirsPrice.Count; i++)
            {
                itemDatabase.itemList[i + elixirsIndex[0]].itemPrice = elixirsPrice[i];
            }
        }

        // 아이템DB로부터 엘릭서를 읽어와 도감 딕셔너리에 저장
        private void SetElixirs()
        {
            for (int i = 0; i < itemDatabase.itemList.Count; i++)
            {
                if (itemDatabase.itemList[i].itemID - identifier > 0)
                {
                    elixirsIndex.Add(i);
                    elixirsPrice.Add(itemDatabase.itemList[i].itemPrice);
                }
            }
        }
        #endregion
    }
}