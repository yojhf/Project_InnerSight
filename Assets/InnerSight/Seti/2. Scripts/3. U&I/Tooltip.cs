using Noah;
using UnityEngine;

namespace InnerSight_Seti
{
    /// <summary>
    /// ����ǥ UI�� ����� �����ϴ� Ŭ����
    /// </summary>
    public class Tooltip : MonoBehaviour
    {
        // �ʵ�
        #region Variables
        // �����ͺ��̽� �Ҵ�
        [SerializeField]
        private ItemDatabase itemDatabase;

        // �ʵ�
        private int itemID;
        private ShelfStorage itemStorage;
        private SpriteRenderer itemSprite;
        #endregion

        // ������ ����Ŭ
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