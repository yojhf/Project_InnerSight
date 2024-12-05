using Noah;
using UnityEngine;

namespace InnerSight_Seti
{
    /// <summary>
    /// 꼬리표 UI의 기능을 관리하는 클래스
    /// </summary>
    public class Tooltip : MonoBehaviour
    {
        // 필드
        #region Variables
        // 데이터베이스 할당
        [SerializeField]
        private ItemDatabase itemDatabase;

        // 필드
        private int itemID;
        private ShelfStorage itemStorage;
        private SpriteRenderer itemSprite;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Awake()
        {
            itemSprite = transform.GetChild(1).GetComponent<SpriteRenderer>();

            itemStorage = GetComponentInParent<ShelfStorage>();
            itemID = itemStorage.keyId;
            itemSprite.sprite = itemDatabase.itemList.Find(key => key.itemID == itemID).itemImage;
        }
        #endregion
    }
}