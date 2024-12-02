using InnerSight_Seti;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ShelfStorage : MonoBehaviour
{
    public List<Transform> items = new List<Transform>();
    public int keyId = 1;

    private bool isStroage = false;
    Transform item;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

    void AddObject()
    {
        if (isStroage)
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
        }
    }

    void RemoveObject()
    {
        items[items.Count - 1].GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Item item = collision.transform.GetComponent<Item>();

        if (item != null)
        {
            if (item.ItemId == keyId)
            {
                Debug.Log("aaaa");
                isStroage = true;

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
