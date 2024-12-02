// ShelfSnapZone.cs
using UnityEngine;

public class ShelfSnapZone : MonoBehaviour
{
    [Header("Snap Settings")]
    public Transform snapPoint; // ���� ��ġ
    public float snapDistance = 0.2f; // ���� ��� �Ÿ�
    public MeshRenderer shelfRenderer; // ������ Mesh Renderer

    private GameObject currentItemOnShelf; // ���� ���ݿ� �ִ� ����

    private void Start()
    {
        if (shelfRenderer != null)
        {
            shelfRenderer.enabled = false; // �ʱ⿡�� ��Ȱ��ȭ
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

        // ���� ��ġ ����
        obj.transform.position = snapPoint.position;
        obj.transform.rotation = snapPoint.rotation;
        obj.GetComponent<Rigidbody>().isKinematic = true;

        // ���� MeshRenderer Ȱ��ȭ
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

            // ���� MeshRenderer ��Ȱ��ȭ
            if (shelfRenderer != null)
            {
                shelfRenderer.enabled = false;
            }

            Debug.Log("Object removed from shelf!");
        }
    }
}
