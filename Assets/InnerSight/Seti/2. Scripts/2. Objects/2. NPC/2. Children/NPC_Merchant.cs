using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace InnerSight_Seti
{
    public static class NPC_State
    {
        public static string IsEntered = "IsEntered";
        public static string Distance = "Distance";
        public static string SelfDistance = "SelfDistance";
    }

    // NPC �� ������ �θ� Ŭ����
    public abstract class NPC_Merchant : NPC
    {
        // �ʵ�
        #region Variables
        // ��ǳ�� �ʵ�
        protected string talk = "";
        [SerializeField]
        protected Transform head;
        protected TextMeshPro talkText;
        protected GameObject talkBubble;
        [SerializeField]
        protected GameObject talkBubblePrefab;

        // �մ� ����� �ʵ�
        protected Vector3 initialPosition;  // ���� ��ġ
        protected float selfDistance;       // ���� ��ġ���� �Ÿ�
        protected float distance;           // �÷��̾���� �Ÿ�
        protected Dictionary<ItemKey, ItemValue> tradeDict = new();

        // �ܼ� ����
        protected int buyCost;
        protected int sellCost;

        // �Ҹ��� ����
        protected bool isEntered;
        protected bool canTrade;

        // ������Ʈ
        protected Animator animator;

        // Ŭ���� ������Ʈ
        protected Player player;
        protected ShopBag bag;
        #endregion

        // ������ ����Ŭ
        #region Life Cycle
        private void Start()
        {
            animator = GetComponent<Animator>();
            talkBubble = Instantiate(talkBubblePrefab,
                                                      head.position + new Vector3(0.25f, 0, 0.5f),
                                                      Quaternion.identity,
                                                      head);
            talkText = talkBubble.GetComponentInChildren<TextMeshPro>();
            talkBubble.SetActive(false);
        }
        #endregion

        // �޼���
        #region Methods
        // ��ȣ�ۿ� �޼���, ������ ��� �������� ���ſ� �Ǹ� ��
        public override void Interaction()
        {

        }

        // AI �ൿ ��ƾ�� �����ϴ� �޼���
        protected override void AIBehaviour()
        {

        }

        // ��ȭ �޼���
        protected virtual void Dialogue(string talk, float delay)
        {
            StopAllCoroutines();
            StartCoroutine(TalkBubble(talk, delay));
        }

        protected virtual IEnumerator TalkBubble(string talk, float delay)
        {
            talkBubble.SetActive(true);
            talkText.text = talk;

            yield return new WaitForSeconds(delay);
            talkBubble.SetActive(false);

            yield break;
        }

        // ���� �ŷ� �޼���
        public virtual void Trade(Player player)
        {
            // NPC���� ���� �ִ� ��縦 ����!

            // �Ǹ�
            if (bag.shopDict.Count == 0)
            {
                if (tradeDict.Count == 0) TradeStart(player);

                else TradeEnd(player);
            }

            // ����
            else
            {
                CalCost();
                canTrade = player.playerStates.SpendColl(buyCost);
                if (canTrade) GiveItems(player);
                else buyCost = 0;
            }
        }

        // �÷��̾�� ������ �������� �ִ� �޼���
        protected void GiveItems(Player player)
        {
            InventoryManager playerInven = player.PlayerUse.InventoryManager;

            foreach (var items in bag.shopDict)
            {
                playerInven.AddItem(items.Key, items.Value.itemCount);
                playerInven.Inventory.invenDict[items.Key].Count(items.Value.itemCount - 1);
            }

            foreach (var slot in bag.shopItems)
                Destroy(slot);
        }

        // ��ٱ��� �������� ������ ����ϴ� �޼���
        protected void CalCost()
        {
            foreach (var item in bag.shopDict)
                buyCost += item.Key.itemPrice * item.Value.itemCount;
        }

        // �÷��̾��� ������ �Ǹ� ����
        protected virtual void TradeStart(Player player)
        {
            InventoryManager inven = player.PlayerUse.InventoryManager;

            if (inven.Inventory.invenDict.Count == 0) return;

            inven.SetNPC(this);

            // ��ٱ��ϸ� �ݰ�
            player.PlayerTrade.TakeOffShopBag();

            // �κ��丮�� ����
            inven.IsOnTrade = true;
            player.control.Player.Inventory.Disable();
            if (!inven.IsOpenInventory) inven.ForTrade(true);

            // �Ǹſ� UI�� ����
            // �����ϸ� ������ �ٲٰ�
            // esc Ű�� ������ ��ҵ� �����ؾ� �ϰ�
            // ����ϰ� �Ǹ� NPC�� ��絵 �ؾ���
        }

        // NPC�� �÷��̾��� �������� �ϳ��� �Ѱܹ����� ��ο� ���
        public void TradeSetItem(ItemKey tradeItem)
        {
            if (tradeDict.ContainsKey(tradeItem))
                tradeDict[tradeItem].Count(1);

            else
            {
                ItemValue itemValue = new();
                tradeDict.Add(tradeItem, itemValue);
                itemValue.itemIndex = tradeDict.Count - 1;
            }
        }

        // �ŷ� ����
        protected void TradeEnd(Player player)
        {
            InventoryManager inven = player.PlayerUse.InventoryManager;

            foreach (var item in tradeDict)
                sellCost += item.Key.itemSellPrice * item.Value.itemCount;

            player.playerStates.EarnColl(sellCost);

            // �κ��丮�� �ٽ� �ݰ�
            if (inven.Inventory.invenDict.Count != 0)
                inven.ForTrade(false);
            player.control.Player.Inventory.Enable();
            inven.IsOnTrade = false;

            // ��ٱ��ϸ� �ٽ� ����
            player.PlayerTrade.GetShopBag();

            inven.SetNPC(null);
        }

        // �ʱ�ȭ
        protected void Initialize()
        {
            buyCost = 0;
            sellCost = 0;
            tradeDict.Clear();
            bag.shopDict.Clear();
            bag.shopItems.Clear();
        }
        #endregion

        // �̺�Ʈ �޼���
        #region Event Methods
        public virtual void PlayerEnter(Collider other)
        {
            initialPosition = transform.position;
            
            player = other.transform.GetComponent<Player>();
            player.PlayerTrade.GetShopBag();
            bag = player.PlayerTrade.ShopBag;

            isEntered = true;
            player.playerStates.isShopEnter = isEntered;
            animator.SetBool(NPC_State.IsEntered, isEntered);
        }

        public virtual void PlayerStay(Collider other)
        {
            selfDistance = Vector3.Distance(initialPosition, this.transform.position);
            animator.SetFloat(NPC_State.SelfDistance, selfDistance);

            distance = Vector3.Distance(this.transform.position, other.transform.position);
            animator.SetFloat(NPC_State.Distance, distance);
        }

        public virtual void PlayerExit(Collider _)
        {
            isEntered = false;
            player.playerStates.isShopEnter = isEntered;
            animator.SetBool(NPC_State.IsEntered, isEntered);

            player.PlayerTrade.TakeOffShopBag();
            player = null;
        }
        #endregion
    }
}