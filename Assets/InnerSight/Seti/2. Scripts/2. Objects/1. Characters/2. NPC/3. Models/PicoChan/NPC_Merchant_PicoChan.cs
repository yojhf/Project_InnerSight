using UnityEngine;

namespace InnerSight_Seti
{
    public class NPC_Merchant_PicoChan : NPC_Merchant
    {
        // �ʵ�
        #region Variables
        private Vector3 walkDirection;
        #endregion

        // �޼���
        #region Methods
        // ��ȣ�ۿ� �޼���, ������ ��� �������� ���ſ� �Ǹ� ��
        public override void Interaction()
        {
            /*
            // ��ȣ�ۿ��� ����
            // ��� NPC ����
            3. �� �ٰ����� �ڵ����� ��ȣ�ۿ� UI ����
            3-1. ���� �ٽ� �־����� ��ȣ�ۿ� UI�� ������ �÷��̾�� �߰���� �λ�

            // ���� NPC ���� - ��ȣ�ۿ� UI(����/�Ǹ�/����/����Ʈ ��)����
            4. ���Ÿ� �����ϸ� ���ʿ� �κ��丮 â�� ������ �ش� NPC�� �Ǹ� ���� �������� �����ش�
            5. �ǸŸ� �����ϸ� ���� ���ʿ� �κ��丮 â�� ������ �巡�׾ص���̳� ���콺 ������ Ŭ������ �ǸŰ� ����
            6. ���� ��ư�� Ŭ���ϸ� ���� ȹ��
            7. 3������ ���ư��� "�� �ʿ��� �� ��������?"��� ��簡 ���

            *8. 4-7 ������ escŰ�� ������ ��Ұ� ����
            *8-1. NPC�� �ٽ� �ൿ��ƾ���� ���ư�, �÷��̾�� ���� ������ ����
            *8-2. ������ ������ �÷��̾ ���� �ȿ� �������� �����ϰ� �ִ� ����
            *8-3. Ʈ���� �ݶ��̴��� ���Ǿ��� �ƴ϶� ������ �ο��ϰ� �̸� NPC�� ������ �� ������ �� ��?

            *9. �÷��̾ ������ �޼��ϰ� ������ ������ 3-1�� ���ư� �ۺ� �λ�
            *9-1. �湮���� �� ������ �ʾƼ� �� �湮�ϸ� "�� ���̳׿�?"��� �λ��� �ٲ�� ���� ��
             */
        }

        // AI �ൿ ��ƾ�� �����ϴ� �޼���
        protected override void AIBehaviour()
        {
            // �ൿ ��ƾ�� ����
            /*1.ȣ���� �ϰų�, ������ �ڴ� �� �����ο� �ൿ ��ƾ�� �����ٰ�
            2.�÷��̾ �ٰ����� �ൿ�� ���߰� �ٰ��ͼ� �÷��̾�� �λ�*/
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

            // NPC���� ���� �ִ� ��縦 ����!
            if (bag.shopDict.Count == 0)
            {
                if (tradeDict.Count == 0)
                {
                    InventoryManager inven = player.PlayerUse.InventoryManager;
                    if (inven.Inventory.invenDict.Count == 0)
                    {
                        Dialogue("���� ���� ���� �������� �ϳ��� ���µ�?", 5);
                        return;
                    }

                    Dialogue("�ƹ��͵� �� �����?\n�׷� �� �ȷ� �� �ž�?", 5);
                }

                else
                {
                    Dialogue($"{sellCost}�� �ָ� �Ǵ� ����?", 5);
                }
            }

            else
            {
                if (canTrade)
                    Dialogue($"{buyCost}�ݸ� ��!", 5);
                else
                {
                    Dialogue($"����, �� ���� ���ڶ��ݾ�!", 5);
                    return;
                }
            }

            Initialize();
        }
        #endregion

        // �̺�Ʈ �޼���
        #region Event Methods
        private void OnAnimatorIK(int layerIndex)
        {
            if (animator)
            {
                if (isEntered)
                {
                    // ����� IK ����ġ ����
                    animator.SetLookAtWeight(1.0f);  // 0~1 ���� ��, 1�ϼ��� �����ϰ� �ٶ�

                    // ī�޶� ���� ��� ��ġ�� ���� ��ǥ�� ����Ͽ� �ٶ󺸴� ��ġ�� ����
                    Vector3 lookAtPosition = player.transform.position + Vector3.up * 1.212f;  // ī�޶� �ٶ󺸴� ������ �������� 10 ���� ���� ������ ����

                    // ��尡 �ٶ� ��ġ ����
                    animator.SetLookAtPosition(lookAtPosition);
                }

                else animator.SetLookAtWeight(0f);
            }
        }

        public override void PlayerEnter(Collider other)
        {
            base.PlayerEnter(other);

            if (player.playerStates.isBoard != null)
                Dialogue("���� �� ���Կ� �װ� Ÿ�� �����ٴ�!", 5);
            else Dialogue("�ȳ�!\n������ �� �緯 �� �ž�?", 5);
        }

        public override void PlayerStay(Collider other)
        {
            base.PlayerStay(other);
        }

        public override void PlayerExit(Collider _)
        {
            base.PlayerExit(_);

            Dialogue("������ �� ��!", 5);
        }
        #endregion

        // ��ƿ��Ƽ
        #region Utilities
        /*IEnumerator Walking(Vector3 targetPosition, float speed)
        {
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }
            transform.position = targetPosition; // ��ǥ ������ ���������� ��Ȯ�ϰ� ��ġ�� ������

            int randomState = Random.Range(0, 5);
            animator.SetInteger("Idle", randomState);

            yield break;
        }*/
        #endregion
    }
}