using UnityEngine;

namespace MyPet.AI
{
    public class AttackState : State<AnimalCon>
    {
        private Animator animator;

        protected int isAttackHash = Animator.StringToHash("isAttack");

        public override void Init()
        {
            animator = context.GetComponent<Animator>();
        }
        public override void OnEnter()
        {
            animator.SetBool(isAttackHash, true);
        }
        public override void Update(float deltaTime)
        {

        }
        public override void OnExit()
        {
            animator.SetBool(isAttackHash, false);
        }
    }
}