// File: GrabbableItem.cs
using UnityEngine;

public class GrabbableItem : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnSelectEnter()
    {
        rb.isKinematic = true; // ������ ���� �� ���� ȿ�� ��Ȱ��ȭ
        Debug.Log("Object picked up!");
    }

    public void OnSelectExit()
    {
        rb.isKinematic = false; // ������ ���� �� ���� ȿ�� Ȱ��ȭ
        Debug.Log("Object released!");
    }
}
