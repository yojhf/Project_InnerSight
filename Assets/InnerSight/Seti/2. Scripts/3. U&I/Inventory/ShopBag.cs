using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace InnerSight_Seti
{
    // �������� �÷��̾ ������ �������� �����ϴ� Ŭ����
    public class ShopBag : MonoBehaviour
    {
        // �ʵ�
        #region Variables
        // Ŭ���� ������Ʈ
        private PlayerTrade playerTrade;

        // ��ٱ��� ���
        private const float alphaValue = 0.9f;      // ��ٱ��Ͽ� ��� �������� ���İ�
        private RectTransform shopBagTransform;     // ��ٱ��� ������
        [SerializeField]
        private GameObject shopItem;                // ���� ������
        public List<GameObject> shopItems = new();  // ��ٱ��� ����Ʈ
        public Dictionary<ItemKey, ItemValue> shopDict = new();
        #endregion

        // �Ӽ�
        #region Properties
        public PlayerTrade PlayerTrade => playerTrade;
        #endregion

        // �޼���
        #region Methods
        public void SetPlayer(PlayerTrade playerTrade)
        {
            this.playerTrade = playerTrade;
            shopBagTransform = this.transform.GetChild(0) as RectTransform;
        }

        // ��ٱ��Ͽ� �� �������� �߰��� �� �� ������ �����ϴ� �޼���
        public void MakeSlot(ItemKey itemKey, ItemValue itemValue)
        {
            // �������� �߰��ϰ�
            shopDict.Add(itemKey, itemValue);

            // ������ �������� ���� �ε����� ����
            itemValue.itemIndex = shopDict.Count - 1;

            // �� ���� UI ������Ʈ�� ����
            shopItems.Add(Instantiate(shopItem, shopBagTransform));

            // ���� ���� ������Ʈ
            UpdateSlot(itemKey, itemValue.itemIndex);

            // ���� ���ġ
            MathUtility.ReArrObjects(80, shopItems);

            // OnClick ������ �ڵ� ���
            AssignSlots();
        }

        // ������ �����ϴ� �޼���
        private void TakeSlot(ItemKey selectedItemKey)
        {
            foreach (KeyValuePair<ItemKey, ItemValue> pair in shopDict)
            {
                if (pair.Key == selectedItemKey)
                {
                    // ���� ����
                    Destroy(shopItems[pair.Value.itemIndex]);
                    shopItems.RemoveAt(pair.Value.itemIndex);
                }

                else if (pair.Value.itemIndex > shopDict[selectedItemKey].itemIndex)
                    pair.Value.itemIndex--;
            }

            // �ش� Element ����
            shopDict.Remove(selectedItemKey);
        }

        // ������ ������Ʈ�ϴ� �޼���
        private void UpdateSlot(ItemKey itemKey, int slotIndex)
        {
            // ������ ������Ʈ
            Image image = shopItems[slotIndex].GetComponent<Image>();
            image.overrideSprite = itemKey.itemImage;

            // ������Ʈ �� �������� ���İ� ����
            ColorUtility.SetAlpha(image, alphaValue);

            CountItem(itemKey, slotIndex);
        }

        // �� ��ư�� ��ȣ�� �´� �ε����� �ڵ����� �Ҵ��ϴ� �޼���
        private void AssignSlots()
        {
            foreach (var pair in shopDict)
            {
                int index = pair.Value.itemIndex;  // �� ������ �ݵ�� ���� ��������� ���� �Լ� �ȿ��� �ùٸ��� �۵�

                shopItems[index].GetComponent<Button>().onClick.RemoveAllListeners();
                shopItems[index].GetComponent<Button>().onClick.AddListener(() => TakeSlot(pair.Key));
            }
        }

        // ������ ������ ǥ���ϴ� �޼���
        public void CountItem(ItemKey itemKey, int index)
        {
            // �������� ������ 2�� �̻��̶�� ������ ǥ���ϰ�
            if (shopDict[itemKey].itemCount > 1)
                shopItems[index].GetComponentInChildren<TextMeshProUGUI>().text = shopDict[itemKey].ItemCount;

            // 1����� ǥ�� �� ��
            else
                shopItems[index].GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
        #endregion
    }
}