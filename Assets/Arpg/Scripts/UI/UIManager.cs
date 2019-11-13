using System;
using UnityEngine;

namespace Arpg.Scripts.UI
{
    public class UIManager:MonoBehaviour
    {
        public static UIManager instance;
        public Transform informationRoot;
        
        private void Awake()
        {
            instance = this;
        }
        
        
        
    }
}