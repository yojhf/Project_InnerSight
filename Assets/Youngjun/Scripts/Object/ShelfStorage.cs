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

            //StartCoroutine(Test());
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

                Debug.Log("11");

                playerSetting.PlayerInteraction.interactables.Clear();

                Destroy(colItem);

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
            for (int i = items.Count; i >= 0; i--)
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


        // Test
        IEnumerator Test()
        { 
            yield return new WaitForSeconds(3f);

            while (true)
            {
                RemoveObject();

                yield return null;
            }


        }

        //private void OnTriggerEnter(Collider other)
        //{
        //    Item item = other.transform.GetComponent<Item>();

        //    if (item != null)
        //    {
        //        if (item.ItemId == keyId)
        //        {
        //            isStroage = true;

        //            //grapItem = collision.gameObject;
        //            AddObject(other.gameObject);

        //        }
        //    }
        //}

        private void OnTriggerStay(Collider other)
        {
            Item item = other.transform.GetComponent<Item>();

            if (item != null)
            {
                if (item.ItemId == keyId)
                {
                    isStroage = true;

                    //grapItem = collision.gameObject;
                    AddObject(other.gameObject);

                }

            }
        }

        private void OnTriggerExit(Collider other)
        {
            Item item = other.transform.GetComponent<Item>();

            if (item != null)
            {
                isStroage = false;
            }
        }

        //private void OnCollisionEnter(Collision collision)
        //{
        //    Item item = collision.transform.GetComponent<Item>();

        //    if (item != null)
        //    {
        //        if (item.ItemId == keyId)
        //        {
        //            isStroage = true;

        //            //grapItem = collision.gameObject;

        //            AddObject(collision.gameObject);

        //        }
        //    }
        //}
        //private void OnCollisionExit(Collision collision)
        //{
        //    Item item = collision.transform.GetComponent<Item>();

        //    if (item != null)
        //    {
        //        isStroage = false;
        //    }
        //}

    }
}