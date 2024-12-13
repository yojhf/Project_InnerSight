using UnityEngine;

namespace MyPet.AI
{
    public class CatCon : AnimalCon
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            // StateMachine ����, IdleState() ���
            base.Start();

            // ����� ������ ���� �߰� ���
            stateMachine.AddState(new SitState());
            stateMachine.AddState(new DrinkState());
            stateMachine.AddState(new AttackState());
            stateMachine.AddState(new BackSleepState());
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }

        public void Idle()
        {
            ChangeState<IdleState>();
        }
        public void Sit()
        {
            ChangeState<SitState>();
        }

        public void Drink()
        {
            ChangeState<DrinkState>();
        }

        public void Attack()
        {
            ChangeState<AttackState>();
        }
        public void BackSleep()
        {
            ChangeState<BackSleepState>();
        }



    }
}