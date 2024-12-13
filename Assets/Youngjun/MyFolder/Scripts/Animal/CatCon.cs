using UnityEngine;

namespace MyPet.AI
{
    public class CatCon : AnimalCon
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            // StateMachine 생성, IdleState() 등록
            base.Start();

            // 고양이 고유의 상태 추가 등록
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