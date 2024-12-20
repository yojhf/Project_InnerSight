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

                // 상호작용 대상이 실제로 존재하면
                if (closestObject != null)
                {
                    // 인터페이스로부터 itemData 가져오기
                    if (closestObject.TryGetComponent<IInteractable>(out var interactable))
                    {
                        if (playerSetting.PlayerUse.InventoryManager.Inventory.invenDict.Count >=
                            playerSetting.PlayerUse.InventoryManager.Inventory.invenSlots.Length)
                        {
                            Debug.Log("인벤토리가 꽉찼어!");
                            return;
                        }
                        else
                        {
                            // 인벤토리에 더하는 메서드 호출
                            ItemKey itemData = interactable.GetItemData();
                            playerSetting.PlayerUse.InventoryManager.AddItem(itemData, 1);

                            // 도감 갱신
                            //recipeManager.IdentifyRecipe(itemData);
                        }
                    }

                    // 해당 오브젝트 줍기
                    Destroy(closestObject);

                    // Dictionary에서 해당 오브젝트와 UI 제거
                    autoInteractables.Remove(closestObject);
                }
            }
        }

        // "저게 뭐지?" 상호작용 화살표 생성 메서드
        private void WhatIsThat(Collider other)
        {
            // 인터페이스로 상호작용 가능한 오브젝트만 구별
            if (other.gameObject.GetComponent<IInteractable>() == null) return;

            // 해당 오브젝트가 이미 리스트에 있는지 확인
            if (!autoInteractables.Contains(other.gameObject))
            {
                // 상호작용 가능한 오브젝트 추가
                autoInteractables.Add(other.gameObject);
            }
        }

        // "쓰레기네." 상호작용 화살표 제거 메서드
        private void Sucks(Collider other)
        {
            // 인터페이스로 상호작용 가능한 오브젝트만 구별
            if (other.gameObject.GetComponent<IInteractable>() == null) return;

            // 해당 오브젝트가 리스트에 있는지 확인
            if (autoInteractables.Contains(other.gameObject))
            {
                // List에서 해당 오브젝트 제거
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
