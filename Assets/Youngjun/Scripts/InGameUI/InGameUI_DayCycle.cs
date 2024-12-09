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
        private float speed = 5000f;
        private int dayTax = 10;
        private int stopTax = 1000;

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
        }

        public void DayResetUI()
        {
            resetUI.SetActive(true);

            StartCoroutine(StartReset_Co());

        }

        IEnumerator StartReset_Co()
        {
            int m_Gold = PlayerStats.Instance.CurrentGold + PlayerStats.Instance.RevenueGold;
            int m_Tax = PlayerStats.Instance.RevenueGold / dayTax;
            int m_ShopTax = stopTax;
            int m_TotalGold = (m_Gold + (PlayerStats.Instance.RevenueGold - m_Tax)) - m_Tax - m_ShopTax;


            myGold.gameObject.SetActive(true);

            StartCoroutine(GoldCount(m_Gold, myGold, "MyGold : "));

            yield return new WaitForSecondsRealtime(1f);

            tax.gameObject.SetActive(true);

            StartCoroutine(GoldCount(m_Tax, tax, "Tax : "));

            yield return new WaitForSecondsRealtime(1f);

            shopTax.gameObject.SetActive(true);

            StartCoroutine(GoldCount(m_ShopTax, shopTax, "ShopTax : "));

            yield return new WaitForSecondsRealtime(1f);

            totalGold.gameObject.SetActive(true);

            StartCoroutine(GoldCount(m_TotalGold, totalGold, "TotalGold : "));

            PlayerStats.Instance.SetGold(m_TotalGold);

            yield return new WaitForSecondsRealtime(1f);

            ActiveCon(false);

            resetUI.gameObject.SetActive(false);
        }

        void ActiveCon(bool active)
        {
            myGold.gameObject.SetActive(active);
            tax.gameObject.SetActive(active);
            shopTax.gameObject.SetActive(active);
            totalGold.gameObject.SetActive(active);
        }

        IEnumerator GoldCount(int num, TMP_Text count, string text)
        {
            float ctime = 0;

            while (ctime <= num)
            {
                ctime += Time.unscaledDeltaTime * speed;
                count.text = text + ctime.ToString("F0");
                yield return null;

            }

            count.text = text + num.ToString("F0");
        }
    }

}
