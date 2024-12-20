using InnerSight_Seti;
using System.Collections.Generic;
using UnityEngine;

namespace Noah
{
    public class GetAutoItem : MonoBehaviour
    {
        private Transform player;
        public List<GameObject> autoInteractables = new();

        PlayerSetting playerSetting;

        private void Start()
        {
            player = transform.parent;
            playerSetting = player.GetComponent<PlayerSetting>();

        }

        private void Update()
        {
            if (gameObject != null)
            {
                if (InputActManager.Instance.IsStorage() && PlayerStats.Instance.OnAutoLoot)
                {
                    ThisIsMineAuto();
                }
            }
        }

        // Auto Get
        public void ThisIsMineAuto()
        {
            if (gameObject != null || autoInteractables != null)
            {
                GameObject closestObject = MathUtility.MinDisObject(gameObject, autoInteractables);

                // ��ȣ�ۿ� ����� ������ �����ϸ�
                if (closestObject != null)
                {
                    // �������̽��κ��� itemData ��������
                    if (closestObject.TryGetComponent<IInteractable>(out var interactable))
                    {
                        if (playerSetting.PlayerUse.InventoryManager.Inventory.invenDict.Count >=
                            playerSetting.PlayerUse.InventoryManager.Inventory.invenSlots.Length)
                        {
                            Debug.Log("�κ��丮�� ��á��!");
                            return;
                        }
                        else
                        {
                            // �κ��丮�� ���ϴ� �޼��� ȣ��
                            ItemKey itemData = interactable.GetItemData();
                            playerSetting.PlayerUse.InventoryManager.AddItem(itemData, 1);

                            // ���� ����
                            //recipeManager.IdentifyRecipe(itemData);
                        }
                    }

                    // �ش� ������Ʈ �ݱ�
                    Destroy(closestObject);

                    // Dictionary���� �ش� ������Ʈ�� UI ����
                    autoInteractables.Remove(closestObject);
                }
            }
        }

        // "���� ����?" ��ȣ�ۿ� ȭ��ǥ ���� �޼���
        private void WhatIsThat(Collider other)
        {
            // �������̽��� ��ȣ�ۿ� ������ ������Ʈ�� ����
            if (other.gameObject.GetComponent<IInteractable>() == null) return;

            // �ش� ������Ʈ�� �̹� ����Ʈ�� �ִ��� Ȯ��
            if (!autoInteractables.Contains(other.gameObject))
            {
                // ��ȣ�ۿ� ������ ������Ʈ �߰�
                autoInteractables.Add(other.gameObject);
            }
        }

        // "�������." ��ȣ�ۿ� ȭ��ǥ ���� �޼���
        private void Sucks(Collider other)
        {
            // �������̽��� ��ȣ�ۿ� ������ ������Ʈ�� ����
            if (other.gameObject.GetComponent<IInteractable>() == null) return;

            // �ش� ������Ʈ�� ����Ʈ�� �ִ��� Ȯ��
            if (autoInteractables.Contains(other.gameObject))
            {
                // List���� �ش� ������Ʈ ����
                autoInteractables.Remove(other.gameObject);
            }
        }



        private void OnTriggerEnter(Collider other)
        {
            Item itemKey = other.GetComponent<Item>();

            if (itemKey != null)
            {
                WhatIsThat(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Sucks(other);
        }
    }

}
