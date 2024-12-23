using InnerSight_Kys;
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
    public GameObject successEffect;
    public GameObject failEffect;


    [SerializeField] private int mixCount = 2;
    [SerializeField] private Transform spwanPos;
    [SerializeField] private Transform spwanPos2;
    [SerializeField] private Transform effectPos;
    public List<Item> objects = new List<Item>();

    private bool isCanMix = false;

    //ItemKey itemKey;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //effectPos = transform.GetChild(0);
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
                AudioManager.Instance.Play("ElixerSucceed");

                GameObject tmp_Effect = Instantiate(successEffect, effectPos.position, Quaternion.identity);

                ResetMix();
                Instantiate(itemKey.GetPrefab(), spwanPos.position, Quaternion.identity);
                Instantiate(itemKey.GetPrefab(), spwanPos2.position, Quaternion.identity);

                Destroy(tmp_Effect, 1f);

                Codex_Recipe_Manager.Instance.IdentifyRecipe(itemKey);
            }
            else
            {
                FailMix();
            }

        }
    }

    void FailMix()
    {
        AudioManager.Instance.Play("Explosion");

        var failItem = itemDataBase.itemList.FirstOrDefault(item => item.itemID == failItemKey);

        GameObject tmp_Effect = Instantiate(failEffect, effectPos.position, Quaternion.identity);

        ResetMix();
        Instantiate(failItem.GetPrefab(), spwanPos.position, Quaternion.identity);
        Instantiate(failItem.GetPrefab(), spwanPos2.position, Quaternion.identity);

        Destroy(tmp_Effect, 1f);
    }

    void ResetMix()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i])
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
        return index > 2000 && index < 6000;
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

            collision.transform.GetComponent<Collider>().enabled = false;
            collision.transform.GetComponent<Rigidbody>().isKinematic = true;

            //collision.gameObject.SetActive(false);

            if (objects.Count >= mixCount)
            {
                if (!Codex_Recipe_Manager.Instance.IsFirstReaction((objects[0].ItemId + objects[1].ItemId)) != null &&
                    RandomCheck(objects[0].ItemId) && RandomCheck(objects[1].ItemId))
                {
                    bool isConform = IsTrueWithProbability(probability);

                    if (isConform)
                    {
                        Debug.Log($"{isConform}");

                        isCanMix = true;
                        MixAble();
                    }
                    else
                    {
                        Debug.Log($"{isConform}");
                        FailMix();
                    }
                }
                else
                {
                    Debug.Log("무조건 성공");

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
