using UnityEngine;

namespace InnerSight_Seti
{
    // NPC 거래용 아이템의 추상 클래스
    public abstract class ItemForTrade : MonoBehaviour
    {
        // 필드
        #region Variables
        // 아이템 식별용 필드
        protected int itemID;
        protected ItemKey itemData;
        public ItemDatabase itemDatabase;

        protected bool isIdentify = false;      // 미확인 해제 판정
        protected Renderer itemRenderer;
        protected Material originMaterial;
        [SerializeField]
        protected Material targettedMaterial;
        [SerializeField]
        protected GameObject interactivePrefab;
        protected GameObject interactiveInstance;
        #endregion

        // 속성
        #region Properties
        public bool IsIdentify => isIdentify;
        #endregion

        // 라이프 사이클
        #region Life Cycle
        protected virtual void Start()
        {
            itemData = itemDatabase.itemList[itemID];

            itemRenderer = this.transform.GetComponent<Renderer>();
            originMaterial = itemRenderer.material;
        }
        #endregion

        // 메서드
        #region Methods
        public ItemKey GetItemData()
        {
            return itemData;
        }

        public void MarkItem()
        {
            if (!isIdentify) itemRenderer.material = targettedMaterial;
            else interactiveInstance.SetActive(true);
        }

        public void ForgetItem()
        {
            if (!isIdentify) itemRenderer.material = originMaterial;
            else interactiveInstance.SetActive(false);
        }

        public void DefineItem()
        {
            if (isIdentify) return;

            isIdentify = true;
            itemRenderer.material = originMaterial;
            interactiveInstance = Instantiate(interactivePrefab,
                                                               this.transform.position + 0.7f * Vector3.up,
                                                               Quaternion.identity);

            if (interactiveInstance.transform.TryGetComponent<NameTag>(out var thisItem))
                thisItem.DefineName(itemData.itemNameKor);
        }
        #endregion
    }
}