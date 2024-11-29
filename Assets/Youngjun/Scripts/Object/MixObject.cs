using InnerSight_Seti;
using System.Collections.Generic;
using UnityEngine;

public class MixObject : MonoBehaviour
{
    public GameObject testPrefab;

    [SerializeField] private int mixCount = 2;
    private Transform spwanPos;
    public List<SampleItem01> objects = new List<SampleItem01>();
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

            ResetMix();

            Instantiate(testPrefab, spwanPos.position, Quaternion.identity);
        }
    }

    void ResetMix()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            Destroy(objects[i].gameObject);
        }

        objects.Clear();
    }

    private void OnCollisionEnter(Collision collision)
    {
        SampleItem01 obj = collision.transform.GetComponent<SampleItem01>();

        if (obj != null)
        {
            if (!objects.Contains(obj))
            {
                objects.Add(obj);
            }

            if (objects.Count == mixCount)
            {
                isCanMix = true;

                MixAble();
            }
        }
    }
}
