using UnityEngine;

namespace InnerSight_Seti
{
    // NPC �ŷ��� �������� �߻� Ŭ����
    public abstract class ItemForTrade : MonoBehaviour
    {
        // �ʵ�
        #region Variables
        // ������ �ĺ��� �ʵ�
        protected int itemID;
        protected ItemKey itemData;
        public ItemDatabase itemDatabase;

        protected bool isIdentify = false;      // ��Ȯ�� ���� ����
        protected Renderer itemRenderer;
        protected Material originMaterial;
        [SerializeField]
        protected Material targettedMaterial;
        [SerializeField]
        protected GameObject interactivePrefab;
        protected GameObject interactiveInstance;
        #endregion

        // �Ӽ�
        #region Properties
        public bool IsIdentify => isIdentify;
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        protected virtual void Start()
        {
            itemData = itemDatabase.itemList[itemID];

            itemRenderer = this.transform.GetComponent<Renderer>();
            originMaterial = itemRenderer.material;
        }
        #endregion

        // �޼���
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