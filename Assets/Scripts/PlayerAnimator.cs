using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimator : MonoBehaviour
{

    const float locomationAnimationSmoothTime = .1f;

    NavMeshAgent agent;
    Animator anim;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        //Debug.Log(agent.velocity.magnitude);
        if (agent.velocity.magnitude == 0)
        {            
            anim.SetBool("isWalking", false);
            agent.updateRotation = false;
        } else if (Mathf.FloorToInt(agent.velocity.magnitude) == 2)
        {
            anim.SetTrigger("Run");
        }
    }
}