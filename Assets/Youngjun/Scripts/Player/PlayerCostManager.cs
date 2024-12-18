using InnerSight_Seti;
using UnityEngine;

namespace Noah
{
    public class PlayerCostManager : Singleton<PlayerCostManager>
    {
        [SerializeField] private int addDayCost;
        private int addShopCost = 20;

        private int dayTax = 10;
        private int shopTax = 200;

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