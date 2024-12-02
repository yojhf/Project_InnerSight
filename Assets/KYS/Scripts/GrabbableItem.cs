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
        rb.isKinematic = true; // 물건을 잡을 때 물리 효과 비활성화
        Debug.Log("Object picked up!");
    }

    public void OnSelectExit()
    {
        rb.isKinematic = false; // 물건을 놓을 때 물리 효과 활성화
        Debug.Log("Object released!");
    }
}
