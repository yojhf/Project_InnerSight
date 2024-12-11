using System;
using UnityEngine;

namespace MyPet.AI
{
    [Serializable]
    public class IdleState : State<AnimalCon>
    {
        private Animator animator;
        //private CharacterController characterController;
        //private NavMeshAgent agent;

        public override void Init()
        {
            animator = context.GetComponent<Animator>();
            //characterController = context.GetComponent<CharacterController>();
            //agent = context.GetComponent<NavMeshAgent>();
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