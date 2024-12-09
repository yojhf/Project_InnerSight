using InnerSight_Seti;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Noah
{
    public class InGameUI_DayCycle : MonoBehaviour
    {
        [SerializeField] private TMP_Text myGold;
        [SerializeField] private TMP_Text tax;
        [SerializeField] private TMP_Text shopTax;
        [SerializeField] private TMP_Text totalGold;

        private GameObject resetUI;
        private float lerpTime = 1f;
        private int dayTax = 10;
        private int stopTax = 10;

        public int DayTax 
        {
            get
            {
                return dayTax;
            }
            set
            { 
                value = dayTax;
            }
        }

        public int StopTax
        {
            get
            {
                return stopTax;
            }
            set
            {
                value = stopTax;
            }
        }


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            resetUI = transform.GetChild(0).gameObject;

            // CurrentGold = shop
        }

        // Update is called once per frame
        void Update()
        {

        }

        void DayResetUI()
        {
            resetUI.SetActive(true);

            StartCoroutine(StartReset_Co());

        }

        IEnumerator StartReset_Co()
        {
            int m_Gold = PlayerStats.Instance.CurrentGold + PlayerStats.Instance.RevenueGold;
            int m_Tax = PlayerStats.Instance.RevenueGold / dayTax;
            int m_ShopTax = PlayerStats.Instance.RevenueGold / stopTax;
            int m_TotalGold = m_Gold + m_Tax + m_ShopTax;


            myGold.gameObject.SetActive(true);

            myGold.text = "MyGold : " + Mathf.Lerp(0, m_Gold, lerpTime).ToString();

            yield return new WaitForSeconds(1f);

            tax.gameObject.SetActive(true);

            tax.text = "Tax : " + Mathf.Lerp(0, PlayerStats.Instance.CurrentGold, lerpTime).ToString();

            yield return new WaitForSeconds(1f);

            shopTax.gameObject.SetActive(true);

            shopTax.text = "ShopTax : " + Mathf.Lerp(0, PlayerStats.Instance.CurrentGold, lerpTime).ToString();

            yield return new WaitForSeconds(1f);

            totalGold.gameObject.SetActive(true);

            totalGold.text = "TotalGold : " + Mathf.Lerp(0, PlayerStats.Instance.CurrentGold, lerpTime).ToString();


           yield return new WaitForSeconds(1f);

            ActiveCon(false);

        }

        void ActiveCon(bool active)
        {
            myGold.gameObject.SetActive(active);
            tax.gameObject.SetActive(active);
            shopTax.gameObject.SetActive(active);
            totalGold.gameObject.SetActive(active);
        }
    }

}
