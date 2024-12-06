using InnerSight_Seti;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MixObject : MonoBehaviour
{
    // 확률
    [Range(0, 100)]
    public float probability = 60f;

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
                FailMix();
            }

        }
    }

    void FailMix()
    {
        var failItem = itemDataBase.itemList.FirstOrDefault(item => item.itemID == failItemKey);

        ResetMix();
        Instantiate(failItem.GetPrefab(), spwanPos.position, Quaternion.identity);
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

    bool IsTrueWithProbability(float chance)
    {
        // Random.Range(0f, 100f)는 0 이상 100 미만의 float 값을 반환
        return Random.Range(0f, 100f) < chance;
    }

    bool RandomCheck(int index)
    {
        return index > 2000 && index < 4000;
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

            

            if (objects.Count >= mixCount)
            {
                if (RandomCheck(objects[0].ItemId) && RandomCheck(objects[1].ItemId))
                {
                    bool isConform = IsTrueWithProbability(probability);

                    Debug.Log($"{isConform}");

                    if (isConform)
                    {
                        isCanMix = true;
                        MixAble();
                    }
                    else
                    {
                        FailMix();
                    }
                }
                else
                {
                    isCanMix = true;
                    MixAble();
                }
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
