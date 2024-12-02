// ShelfSnapZone.cs
using UnityEngine;

public class ShelfSnapZone : MonoBehaviour
{
    [Header("Snap Settings")]
    public Transform snapPoint; // 스냅 위치
    public float snapDistance = 0.2f; // 스냅 허용 거리
    public MeshRenderer shelfRenderer; // 선반의 Mesh Renderer

    private GameObject currentItemOnShelf; // 현재 선반에 있는 물건

    private void Start()
    {
        if (shelfRenderer != null)
        {
            shelfRenderer.enabled = false; // 초기에는 비활성화
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grabbable") && currentItemOnShelf == null)
        {
            float distance = Vector3.Distance(other.transform.position, snapPoint.position);
            if (distance <= snapDistance)
            {
                SnapObject(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentItemOnShelf)
        {
            RemoveObject();
        }
    }

    private void SnapObject(GameObject obj)
    {
        currentItemOnShelf = obj;

        // 물건 위치 스냅
        obj.transform.position = snapPoint.position;
        obj.transform.rotation = snapPoint.rotation;
        obj.GetComponent<Rigidbody>().isKinematic = true;

        // 선반 MeshRenderer 활성화
        if (shelfRenderer != null)
        {
            shelfRenderer.enabled = true;
        }

        Debug.Log("Object snapped to shelf!");
    }

    private void RemoveObject()
    {
        if (currentItemOnShelf != null)
        {
            currentItemOnShelf.GetComponent<Rigidbody>().isKinematic = false;
            currentItemOnShelf = null;

            // 선반 MeshRenderer 비활성화
            if (shelfRenderer != null)
            {
                shelfRenderer.enabled = false;
            }

            Debug.Log("Object removed from shelf!");
        }
    }
}
