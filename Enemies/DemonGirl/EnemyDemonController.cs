using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDemonController : MonoBehaviour
{
    public Transform mainCharacterTransform;
    public Animator animator;


    private NavMeshAgent navMeshAgent;


    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, mainCharacterTransform.position) < 20)
        {
            navMeshAgent.SetDestination(mainCharacterTransform.position);
            animator.SetBool("isEnemyMoving", true);
        }
        else
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
            animator.SetBool("isEnemyMoving", false);
        }
    }
}
