using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRandomSpwan : MonoBehaviour
{
    public List<GameObject> objPrefabs = new List<GameObject>();
    private List<GameObject> spwanObjects = new List<GameObject>();
    public int randomSpCount = 30;
    private GameObject randomSpwanObj;

    BoxCollider boxCollider;

    private void Awake()
    {
        randomSpwanObj = gameObject;

        boxCollider = randomSpwanObj.GetComponent<BoxCollider>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RandomSpwan();

        StartCoroutine(ReSpwanObject());
    }

    Vector3 RandomPos()
    {
        Vector3 originPosition = randomSpwanObj.transform.position;
        // �ݶ��̴��� ����� �������� bound.size ���
        float range_X = boxCollider.bounds.size.x;
        float range_Z = boxCollider.bounds.size.z;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);
        Vector3 RandomPostion = new Vector3(range_X, 0.3f, range_Z);

        Vector3 respawnPosition = originPosition + RandomPostion;
        return respawnPosition;
    }

    void RandomSpwan()
    {
        for (int i = 0; i < randomSpCount; i++)
        {
            int randomIndex = Random.Range(0, objPrefabs.Count);

            Instantiate(objPrefabs[randomIndex], RandomPos(), Quaternion.identity, randomSpwanObj.transform);
        }

        AddSpwanObject();
    }

    void AddSpwanObject()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            spwanObjects.Add(transform.GetChild(i).gameObject);
        }
    }

    void DestorySpwanObject()
    {
        for (int i = 0; i < transform.childCount; i++)
        {       
            Destroy(spwanObjects[i].gameObject);
        }

        spwanObjects.Clear();
    }

    IEnumerator ReSpwanObject()
    {
        // �׽�Ʈ �ð� ��ȯ
        yield return new WaitForSeconds(3f);

        DestorySpwanObject();

        yield return new WaitForSeconds(1f);

        RandomSpwan();
    }



}
