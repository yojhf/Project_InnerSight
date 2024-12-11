using System;
using UnityEngine;

namespace MyPet.AI
{
    [Serializable]
    public class DrinkState : State<AnimalCon>
    {
        private Animator animator;

        protected int isDrinkHash = Animator.StringToHash("isDrink");

        public override void Init()
        {
            animator = context.GetComponent<Animator>();
        }
        public override void OnEnter()
        {
            animator.SetBool(isDrinkHash, true);
        }
        public override void Update(float deltaTime)
        {

        }
        public override void OnExit()
        {
            animator.SetBool(isDrinkHash, false);
        }

    }
}
