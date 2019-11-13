using System;
using AnyPortrait;
using UnityEngine;
using UnityEngine.AI;

namespace Arpg
{
    [RequireComponent(typeof(AgentMonitor))]
    public class PlayerInput:MonoBehaviour
    {
        private AgentMonitor _agentMonitor;

        private void Start()
        {       
            _agentMonitor = this.GetComponent<AgentMonitor>();
            _agentMonitor.SetIsPlayerControl();
            _agentMonitor.updateAction = UpdateAction;
        }

        private void UpdateAction()
        {

            if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Z))
            {
                _agentMonitor.TryAttack();
            }

         
                if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.X))
                {
//                    _agentMonitor.TryJump();

                }
            
            if (_agentMonitor.CanRun)
            {
                var x = Input.GetAxis("Horizontal");
                var z = Input.GetAxis("Vertical");
                if (x != 0 || z != 0)
                {
                    Vector3 moveDir = new Vector3(x,0,z).normalized;
                    _agentMonitor.TryRun(moveDir);
                }
                else
                {
                    _agentMonitor.TryIdle();
                } 
            }
        }
    }
}