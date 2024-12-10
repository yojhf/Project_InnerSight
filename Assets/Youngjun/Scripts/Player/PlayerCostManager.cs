using InnerSight_Seti;
using UnityEngine;

namespace Noah
{
    public class PlayerCostManager : Singleton<PlayerCostManager>
    {
        [SerializeField] private int addShopCost = 100;
        [SerializeField] private int addDayCost;

        private int dayTax = 10;
        private int shopTax = 1000;

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

        public int ShopTax
        {
            get
            {
                return shopTax;
            }
            set
            {
                value = shopTax;
            }
        }


        public void UpdateShopTax()
        {
            shopTax += addShopCost;
        }

        public void UpdateDayTax()
        {
            dayTax += addDayCost;
        }
    }
}