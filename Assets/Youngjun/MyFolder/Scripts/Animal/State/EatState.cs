using System;
using UnityEngine;

namespace MyPet.AI
{
    [Serializable]
    public class EatState : State<AnimalCon>
    {
        private Animator animator;

        public override void Init()
        {
            animator = context.GetComponent<Animator>();
        }
        public override void OnEnter()
        {

        }
        public override void Update(float deltaTime)
        {

        }
        public override void OnExit()
        {

        }
    }
}