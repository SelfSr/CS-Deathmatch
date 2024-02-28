using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class AnimationController_Bot : MonoBehaviour
{
    private Animator animator;
    private Bot Bot;
    private void Start()
    {
        animator = GetComponent<Animator>();
        Bot = GetComponent<Bot>();
    }
    private void Update()
    {
        animator.SetFloat("Speed", Bot.navMeshAgent.velocity.magnitude);
    }
}