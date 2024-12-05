using InnerSight_Seti;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MixObject : MonoBehaviour
{
    public ItemDatabase itemDataBase;
    public PlayerSetting playerSetting;
    public int failItemKey;

    [SerializeField] private int mixCount = 2;
    private Transform spwanPos;
    public List<Item> objects = new List<Item>();

    private bool isCanMix = false;

    ItemKey itemKey;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spwanPos = transform.GetChild(0);
    }


    void MixAble()
    {
        if (isCanMix == true)
        {
            isCanMix = false;

            int mixObjectNum = objects[0].ItemId + objects[1].ItemId;

            var itemKey = itemDataBase.itemList.FirstOrDefault(item => item.itemID == mixObjectNum);

            if (itemKey != null)
            {
                ResetMix();
                Instantiate(itemKey.GetPrefab(), spwanPos.position, Quaternion.identity);
            }
            else
            {
                var failItem = itemDataBase.itemList.FirstOrDefault(item => item.itemID == failItemKey);

                ResetMix();
                Instantiate(failItem.GetPrefab(), spwanPos.position, Quaternion.identity);
            }

        }
    }

    void ResetMix()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            Destroy(objects[i].gameObject);
        }

        if (playerSetting.PlayerInteraction.interactables.Count != 0)
        {
            playerSetting.PlayerInteraction.interactables.Clear();
        }

        objects.Clear();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Item obj = collision.transform.GetComponent<Item>();

        if (obj != null)
        {
            //if (!objects.Contains(obj))
            //{

            //}

            objects.Add(obj);

            if (objects.Count == mixCount)
            {
                isCanMix = true;

                MixAble();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Item obj = collision.transform.GetComponent<Item>();

        if (obj != null)
        {
            objects.Remove(obj);      
        }
    }
}
