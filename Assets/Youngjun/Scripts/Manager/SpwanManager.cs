using InnerSight_Seti;
using System.Collections.Generic;
using UnityEngine;

namespace Noah
{
    public class SpwanManager : Singleton<SpwanManager>
    {
        private List<ObjectRandomSpwan> randomSpwans = new List<ObjectRandomSpwan>();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Init();
        }


        void Init()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                randomSpwans.Add(transform.GetChild(i).GetComponent<ObjectRandomSpwan>());
            }
        }

        public void SpwanCon()
        {
            foreach (var item in randomSpwans)
            {
                item.ResetSpwan();
            }
        }
    }

}
