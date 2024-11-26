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

    // NPC 중 상인의 부모 클래스
    public abstract class NPC_Merchant : NPC
    {
        // 필드
        #region Variables
        // 말풍선 필드
        protected string talk = "";
        [SerializeField]
        protected Transform head;
        protected TextMeshPro talkText;
        protected GameObject talkBubble;
        [SerializeField]
        protected GameObject talkBubblePrefab;

        // 손님 응대용 필드
        protected Vector3 initialPosition;  // 원래 위치
        protected float selfDistance;       // 원래 위치와의 거리
        protected float distance;           // 플레이어와의 거리
        protected Dictionary<ItemKey, ItemValue> tradeDict = new();

        // 단순 변수
        protected int buyCost;
        protected int sellCost;

        // 불리언 변수
        protected bool isEntered;
        protected bool canTrade;

        // 컴포넌트
        protected Animator animator;

        // 클래스 컴포넌트
        protected Player player;
        protected ShopBag bag;
        #endregion

        // 라이프 사이클
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

        // 메서드
        #region Methods
        // 상호작용 메서드, 상인의 경우 아이템의 구매와 판매 등
        public override void Interaction()
        {

        }

        // AI 행동 루틴을 규정하는 메서드
        protected override void AIBehaviour()
        {

        }

        // 대화 메서드
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

        // 최종 거래 메서드
        public virtual void Trade(Player player)
        {
            // NPC마다 개성 있는 대사를 주자!

            // 판매
            if (bag.shopDict.Count == 0)
            {
                if (tradeDict.Count == 0) TradeStart(player);

                else TradeEnd(player);
            }

            // 구매
            else
            {
                CalCost();
                canTrade = player.playerStates.SpendColl(buyCost);
                if (canTrade) GiveItems(player);
                else buyCost = 0;
            }
        }

        // 플레이어에게 구매한 아이템을 주는 메서드
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

        // 장바구니 아이템의 가격을 계산하는 메서드
        protected void CalCost()
        {
            foreach (var item in bag.shopDict)
                buyCost += item.Key.itemPrice * item.Value.itemCount;
        }

        // 플레이어의 아이템 판매 시작
        protected virtual void TradeStart(Player player)
        {
            InventoryManager inven = player.PlayerUse.InventoryManager;

            if (inven.Inventory.invenDict.Count == 0) return;

            inven.SetNPC(this);

            // 장바구니를 닫고
            player.PlayerTrade.TakeOffShopBag();

            // 인벤토리를 열고
            inven.IsOnTrade = true;
            player.control.Player.Inventory.Disable();
            if (!inven.IsOpenInventory) inven.ForTrade(true);

            // 판매용 UI를 열고
            // 가능하면 시점도 바꾸고
            // esc 키를 누르면 취소도 가능해야 하고
            // 취소하게 되면 NPC가 대사도 해야지
        }

        // NPC가 플레이어의 아이템을 하나씩 넘겨받으면 장부에 등록
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

        // 거래 종료
        protected void TradeEnd(Player player)
        {
            InventoryManager inven = player.PlayerUse.InventoryManager;

            foreach (var item in tradeDict)
                sellCost += item.Key.itemSellPrice * item.Value.itemCount;

            player.playerStates.EarnColl(sellCost);

            // 인벤토리를 다시 닫고
            if (inven.Inventory.invenDict.Count != 0)
                inven.ForTrade(false);
            player.control.Player.Inventory.Enable();
            inven.IsOnTrade = false;

            // 장바구니를 다시 연다
            player.PlayerTrade.GetShopBag();

            inven.SetNPC(null);
        }

        // 초기화
        protected void Initialize()
        {
            buyCost = 0;
            sellCost = 0;
            tradeDict.Clear();
            bag.shopDict.Clear();
            bag.shopItems.Clear();
        }
        #endregion

        // 이벤트 메서드
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