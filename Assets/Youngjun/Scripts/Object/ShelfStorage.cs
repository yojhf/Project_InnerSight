using InnerSight_Seti;
using NUnit.Framework;
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

        public void AddObject()
        {
            if (isStroage)
            {
                isStroage = false;

                playerSetting.PlayerInteraction.interactables.Clear();

                Destroy(grapItem);

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
            }
        }

        void RemoveObject()
        {
            items[items.Count - 1].GetComponent<MeshRenderer>().enabled = false;
        }


        // Test
        //void I

        private void OnCollisionEnter(Collision collision)
        {
            Item item = collision.transform.GetComponent<Item>();

            if (item != null)
            {
                if (item.ItemId == keyId)
                {
                    isStroage = true;
                    grapItem = collision.gameObject;


                    if (!charactorAction.IsGrap)
                    {
                        AddObject();
                    }           
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