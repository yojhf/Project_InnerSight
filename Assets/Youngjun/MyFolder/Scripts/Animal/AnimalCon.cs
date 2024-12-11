using UnityEngine;
using UnityEngine.AI;

namespace MyPet.AI
{
    // 동물을 제어하는 클래스 (동물들의 부모 클래스)
    public class AnimalCon : MonoBehaviour
    {
        protected StateMachine<AnimalCon> stateMachine;

        // 참조
        protected Animator animator;
        //protected CharacterController characterController;
        //protected NavMeshAgent agent;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected virtual void Start()
        {
            // StateMachine 생성
            stateMachine = new StateMachine<AnimalCon>(this, new IdleState());
            animator = GetComponent<Animator>();
            //characterController = GetComponent<CharacterController>();
            //agent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            // 현재 상태의 업데이트를 stateMachine의 업데이트를 통해 실행
            stateMachine.Update(Time.deltaTime);
        }

        public R ChangeState<R>() where R : State<AnimalCon>
        {
            return stateMachine.ChangeState<R>();
        }


    }
}