using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TyreAgent : MonoBehaviour
{
    static public TyreAgent inst;

    [HideInInspector]
    public NavMeshAgent agent;

    private void Awake()
    {
        inst = this;
        agent = GetComponent<NavMeshAgent>();
        agent.acceleration = 0;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
    }
}
