using MyPet.AI;
using UnityEngine;

public class DogIdleState : State<AnimalCon>
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
