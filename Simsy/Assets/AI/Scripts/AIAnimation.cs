using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAnimation : MonoBehaviour
{

public Transform AITransform;
NavMeshAgent agent;
Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        NavMeshAgent navMeshAgent= GetComponent<NavMeshAgent>();
        Animator animator= GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = AITransform.position;
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }
}
