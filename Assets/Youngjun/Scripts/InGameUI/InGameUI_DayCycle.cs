using InnerSight_Seti;
using TMPro;
using UnityEngine;

public class InGameUI_DayCycle : MonoBehaviour
{
    [SerializeField] private TMP_Text myGold;
    [SerializeField] private TMP_Text tax;
    [SerializeField] private TMP_Text shopTax;
    [SerializeField] private TMP_Text totalGold;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // CurrentGold = shop
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //void

    void UpdateUI()
    {
        myGold.text = PlayerStats.Instance.CurrentGold.ToString();
        tax.text = PlayerStats.Instance.CurrentGold.ToString();
        shopTax.text = PlayerStats.Instance.CurrentGold.ToString();


        totalGold.text = PlayerStats.Instance.CurrentGold.ToString();

    }
}
