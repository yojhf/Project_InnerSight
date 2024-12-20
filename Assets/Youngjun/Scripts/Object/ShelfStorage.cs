using InnerSight_Kys;
using InnerSight_Seti;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Noah
{
    public class ShelfStorage : MonoBehaviour
    {
        public GameObject player;
        public List<Transform> items = new List<Transform>();
        public int keyId = 1;

        private bool isStroage = false;
        private bool isCanBuy = false;

        public bool IsCanBuy => isCanBuy;

        GameObject grapItem;
        Transform item;

        PlayerSetting playerSetting;
        CharactorAction charactorAction;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            playerSetting = player.GetComponent<PlayerSetting>();
            charactorAction = player.GetComponent<CharactorAction>();

            item = transform.GetChild(0);
            AddListItems();
        }

        void AddListItems()
        {
            for (int i = 0; i < item.childCount; i++)
            {
                items.Add(item.GetChild(i));
            }
        }

        public void AddObject(GameObject colItem)
        {
            if (isStroage && !charactorAction.IsGrap)
            {
                isStroage = false;

                foreach (var item in items)
                {
                    if (item.GetComponent<MeshRenderer>().enabled == true)
                    {
                        continue;
                    }
                    else
                    {
                        item.GetComponent<MeshRenderer>().enabled = true;
                        break;
                    }
                }

                playerSetting.PlayerInteraction.interactables.Clear();

                Destroy(colItem);
            }
        }

        bool IsFull()
        {
            foreach (var item in items)
            {
                if (item.GetComponent<MeshRenderer>().enabled == false)
                {
                    return false;
                }
            }

            return true;
        }

        public void RemoveObject()
        {
            if (items[0].GetComponent<MeshRenderer>().enabled == false)
            {
                isCanBuy = false;
            }
            else
            {
                isCanBuy = true;
            }

            if (isCanBuy)
            {
                for (int i = items.Count - 1; i >= 0; i--)
                {
                    if (items[i].GetComponent<MeshRenderer>().enabled == false)
                    {
                        continue;
                    }
                    else
                    {
                        items[i].GetComponent<MeshRenderer>().enabled = false;
                        break;
                    }
                }
            }
            else
            {

                AudioManager.Instance.Play("SoldOutAlert");
                Debug.Log("재고부족");
                return;
            }

            
        }

        private void OnCollisionStay(Collision collision)
        {
            Item item = collision.transform.GetComponent<Item>();

            if (item != null)
            {
                if (item.ItemId == keyId && !IsFull())
                {
                    isStroage = true;

                    //grapItem = collision.gameObject;
                    AddObject(collision.gameObject);

                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            Item item = collision.transform.GetComponent<Item>();

            if (item != null)
            {
                isStroage = false;
            }
        }

    }
}