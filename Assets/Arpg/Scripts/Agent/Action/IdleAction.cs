using Arpg.Action;
using Arpg.Scripts.Agent;
using UnityEngine.AI;

namespace Arpg.Agent.Action
{
    public class IdleAction:BaseAction,IAction
    {

        public IdleAction(BaseAIGraph aiGraph) : base(aiGraph)
        {
            
        }
        
        public void Start()
        {
            _navMeshAgent.enabled = false;
            aiGraph.TryIdle();
        }

        public void Update()
        {
            
        }


        public void Quit()
        {
           
        }


    }
}