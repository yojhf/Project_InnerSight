using System;
using UnityEngine;

namespace MyPet.AI
{
    [Serializable]
    public class SitState : State<AnimalCon>
    {
        private Animator animator;

        protected int isSitHash = Animator.StringToHash("isSit");

        public override void Init()
        {
            animator = context.GetComponent<Animator>();
        }
        public override void OnEnter()
        {
            animator.SetBool(isSitHash, true);
        }
        public override void Update(float deltaTime)
        {

        }
        public override void OnExit()
        {
            animator.SetBool(isSitHash, false);
        }
    }
}