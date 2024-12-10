using InnerSight_Seti;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Noah
{
    public class InGameUI_DayCycle : MonoBehaviour
    {
        [SerializeField] private TMP_Text curGold;
        [SerializeField] private TMP_Text earnGold;
        [SerializeField] private TMP_Text tax;
        [SerializeField] private TMP_Text shopTax;
        [SerializeField] private TMP_Text totalGold;

        private GameObject resetUI;
        private float speed = 5000f;
     


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
            int m_CurGold = PlayerStats.Instance.CurrentGold;
            int m_RevenueGold = PlayerStats.Instance.RevenueGold;
            int m_Tax = PlayerStats.Instance.RevenueGold / PlayerCostManager.Instance.DayTax;
            int m_ShopTax = PlayerCostManager.Instance.ShopTax;
            int m_TotalGold = (m_CurGold + m_RevenueGold) - m_Tax - m_ShopTax;


            curGold.gameObject.SetActive(true);

            StartCoroutine(GoldCount(m_CurGold, curGold, "MyGold : "));

            yield return new WaitForSecondsRealtime((m_CurGold / speed) + 1f);

            earnGold.gameObject.SetActive(true);

            StartCoroutine(GoldCount(m_RevenueGold, earnGold, "EarnGold : "));

            yield return new WaitForSecondsRealtime((m_RevenueGold / speed) + 1f);

            tax.gameObject.SetActive(true);

            StartCoroutine(GoldCount(m_Tax, tax, "Tax : "));

            yield return new WaitForSecondsRealtime((m_Tax / speed) + 1f);

            shopTax.gameObject.SetActive(true);

            StartCoroutine(GoldCount(m_ShopTax, shopTax, "ShopTax : "));

            yield return new WaitForSecondsRealtime((m_ShopTax / speed) + 1f);

            totalGold.gameObject.SetActive(true);

            StartCoroutine(GoldCount(m_TotalGold, totalGold, "TotalGold : "));

            PlayerStats.Instance.SetGold(m_TotalGold);

            yield return new WaitForSecondsRealtime((m_TotalGold / speed) + 1f);

            ActiveCon(false);

            resetUI.gameObject.SetActive(false);
        }

        void ActiveCon(bool active)
        {
            curGold.gameObject.SetActive(active);
            earnGold.gameObject.SetActive(active);
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
