using UnityEngine;

namespace InnerSight_Seti
{
    public class NPC_Merchant_PicoChan : NPC_Merchant
    {
        // 필드
        #region Variables
        private Vector3 walkDirection;
        #endregion

        // 메서드
        #region Methods
        // 상호작용 메서드, 상인의 경우 아이템의 구매와 판매 등
        public override void Interaction()
        {
            /*
            // 상호작용의 영역
            // 모든 NPC 공통
            3. 더 다가가면 자동으로 상호작용 UI 실행
            3-1. 물론 다시 멀어지면 상호작용 UI가 꺼지고 플레이어에게 잘가라고 인사

            // 상인 NPC 전용 - 상호작용 UI(구매/판매/감정/퀘스트 등)에서
            4. 구매를 선택하면 한쪽에 인벤토리 창이 열리고 해당 NPC가 판매 중인 아이템을 보여준다
            5. 판매를 선택하면 역시 한쪽에 인벤토리 창이 열리고 드래그앤드롭이나 마우스 오른쪽 클릭으로 판매가 가능
            6. 정산 버튼을 클릭하면 콜을 획득
            7. 3번으로 돌아가서 "더 필요한 건 없으세요?"라는 대사가 출력

            *8. 4-7 내용은 esc키를 누르면 취소가 가능
            *8-1. NPC는 다시 행동루틴으로 돌아감, 플레이어는 가게 구경이 가능
            *8-2. 하지만 여전히 플레이어가 상점 안에 들어왔음을 인지하고 있는 상태
            *8-3. 트리거 콜라이더를 엔피씨가 아니라 상점에 부여하고 이를 NPC가 참조할 수 있으면 될 듯?

            *9. 플레이어가 목적을 달성하고 상점을 나서면 3-1로 돌아가 작별 인사
            *9-1. 방문한지 얼마 지나지 않아서 또 방문하면 "또 오셨네요?"라고 인삿말이 바뀌면 좋을 듯
             */
        }

        // AI 행동 루틴을 규정하는 메서드
        protected override void AIBehaviour()
        {
            // 행동 루틴의 영역
            /*1.호객을 하거나, 낮잠을 자는 등 자유로운 행동 루틴을 따르다가
            2.플레이어가 다가가면 행동을 멈추고 다가와서 플레이어에게 인사*/
        }

        /*public void Walk()
        {
            float toX = Random.Range(16.5f, 21.5f);
            float toZ = Random.Range(-8.9f, -1.1f);
            walkDirection = new(toX, 0.3f, toZ);

            Vector3 targetDirection = walkDirection - transform.position;
            transform.localRotation = Quaternion.LookRotation(targetDirection);
            StartCoroutine(Walking(walkDirection, walkSpeed));
        }*/

        public override void Trade(Player player)
        {
            base.Trade(player);

            // NPC마다 개성 있는 대사를 주자!
            if (bag.shopDict.Count == 0)
            {
                if (tradeDict.Count == 0)
                {
                    InventoryManager inven = player.PlayerUse.InventoryManager;
                    if (inven.Inventory.invenDict.Count == 0)
                    {
                        Dialogue("내가 갖고 싶은 아이템이 하나도 없는데?", 5);
                        return;
                    }

                    Dialogue("아무것도 안 골랐네?\n그럼 뭘 팔러 온 거야?", 5);
                }

                else
                {
                    Dialogue($"{sellCost}콜 주면 되는 거지?", 5);
                }
            }

            else
            {
                if (canTrade)
                    Dialogue($"{buyCost}콜만 내!", 5);
                else
                {
                    Dialogue($"뭐야, 너 돈이 모자라잖아!", 5);
                    return;
                }
            }

            Initialize();
        }
        #endregion

        // 이벤트 메서드
        #region Event Methods
        private void OnAnimatorIK(int layerIndex)
        {
            if (animator)
            {
                if (isEntered)
                {
                    // 헤드의 IK 가중치 설정
                    animator.SetLookAtWeight(1.0f);  // 0~1 사이 값, 1일수록 완전하게 바라봄

                    // 카메라가 따라갈 헤드 위치의 월드 좌표를 사용하여 바라보는 위치를 설정
                    Vector3 lookAtPosition = player.transform.position + Vector3.up * 1.212f;  // 카메라가 바라보는 방향을 기준으로 10 유닛 앞의 지점을 지정

                    // 헤드가 바라볼 위치 설정
                    animator.SetLookAtPosition(lookAtPosition);
                }

                else animator.SetLookAtWeight(0f);
            }
        }

        public override void PlayerEnter(Collider other)
        {
            base.PlayerEnter(other);

            if (player.playerStates.isBoard != null)
                Dialogue("감히 내 가게에 그걸 타고 들어오다니!", 5);
            else Dialogue("안녕!\n오늘은 뭘 사러 온 거야?", 5);
        }

        public override void PlayerStay(Collider other)
        {
            base.PlayerStay(other);
        }

        public override void PlayerExit(Collider _)
        {
            base.PlayerExit(_);

            Dialogue("다음에 또 와!", 5);
        }
        #endregion

        // 유틸리티
        #region Utilities
        /*IEnumerator Walking(Vector3 targetPosition, float speed)
        {
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }
            transform.position = targetPosition; // 목표 지점에 도달했으면 정확하게 위치를 맞춰줌

            int randomState = Random.Range(0, 5);
            animator.SetInteger("Idle", randomState);

            yield break;
        }*/
        #endregion
    }
}