using System.Collections;
using System.Collections.Generic;
using AnyPortrait;
using UnityEngine;
using UnityEngine.AI;

public class NewBehaviourScript : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    public Transform testNavTo;
    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        _navMeshAgent.destination = testNavTo.position;
    }
    
}
