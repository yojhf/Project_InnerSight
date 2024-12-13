using UnityEngine;

namespace MyPet.AI
{
    public class SleepState : State<AnimalCon>
    {
        private Animator animator;

        protected int isSleepHash = Animator.StringToHash("isSleep");

        public override void Init()
        {
            animator = context.GetComponent<Animator>();
        }
        public override void OnEnter()
        {
            animator.SetBool(isSleepHash, true);
        }
        public override void Update(float deltaTime)
        {

        }
        public override void OnExit()
        {
            animator.SetBool(isSleepHash, false);
        }
    }
}