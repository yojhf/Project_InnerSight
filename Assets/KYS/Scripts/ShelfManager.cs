// File: ShelfManager.cs
using System.Collections.Generic;
using UnityEngine;

public class ShelfManager : MonoBehaviour
{
    public List<Transform> shelves; // 선반 위치 목록
    private Dictionary<Transform, GameObject> shelfContents = new Dictionary<Transform, GameObject>();

    public bool PlaceItemOnShelf(GameObject item, Transform shelf)
    {
        if (shelfContents.ContainsKey(shelf))
        {
            Debug.LogWarning("Shelf already occupied!");
            return false;
        }

        shelfContents[shelf] = item;
        item.transform.position = shelf.position;
        item.transform.rotation = Quaternion.identity;
        item.GetComponent<Rigidbody>().isKinematic = true;

        Debug.Log("Item placed on shelf!");
        return true;
    }

    public void RemoveItemFromShelf(Transform shelf)
    {
        if (shelfContents.ContainsKey(shelf))
        {
            shelfContents[shelf].GetComponent<Rigidbody>().isKinematic = false;
            shelfContents.Remove(shelf);

            Debug.Log("Item removed from shelf!");
        }
    }
}
