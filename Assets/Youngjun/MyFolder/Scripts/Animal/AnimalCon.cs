using UnityEngine;
using UnityEngine.AI;

namespace MyPet.AI
{
    // ������ �����ϴ� Ŭ���� (�������� �θ� Ŭ����)
    public class AnimalCon : MonoBehaviour
    {
        protected StateMachine<AnimalCon> stateMachine;

        // ����
        protected Animator animator;
        //protected CharacterController characterController;
        //protected NavMeshAgent agent;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected virtual void Start()
        {
            // StateMachine ����
            stateMachine = new StateMachine<AnimalCon>(this, new IdleState());
            animator = GetComponent<Animator>();
            //characterController = GetComponent<CharacterController>();
            //agent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            // ���� ������ ������Ʈ�� stateMachine�� ������Ʈ�� ���� ����
            stateMachine.Update(Time.deltaTime);
        }

        public R ChangeState<R>() where R : State<AnimalCon>
        {
            return stateMachine.ChangeState<R>();
        }


    }
}