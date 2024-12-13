using UnityEngine;

namespace MyPet.AI
{
    public class BackSleepState : State<AnimalCon>
    {
        private Animator animator;

        protected int isBackSleepHash = Animator.StringToHash("isBackSleep");

        public override void Init()
        {
            animator = context.GetComponent<Animator>();
        }
        public override void OnEnter()
        {
            animator.SetBool(isBackSleepHash, true);
        }
        public override void Update(float deltaTime)
        {

        }
        public override void OnExit()
        {
            animator.SetBool(isBackSleepHash, false);
        }
    }
}