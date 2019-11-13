using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Arpg.Scripts.UI
{
    public class InformationUI:MonoBehaviour
    {
        private const string normalInformationUI = "normalInformationUI";
        private AgentMonitor _agentMonitor;
        private RectTransform parent;
        private int maxHealth;
        private int currentHealth;
        public RectTransform frontImage;
        public RectTransform backImage;
        public RectTransform infoText;
        public Vector2 offset;
        
        public static InformationUI NewInstance( AgentMonitor agentMonitor)
        {
            var informationUI =  GameObject.Instantiate(Resources.Load<InformationUI>(normalInformationUI));
            var transform = informationUI.transform;
            transform.parent = UIManager.instance.informationRoot;
            transform.localScale = new Vector3(1,1,1);
            transform.localPosition = new Vector3(0,0,0);
            informationUI._agentMonitor = agentMonitor;
            informationUI.parent = transform.parent.GetComponent<RectTransform>();
            agentMonitor.onPositionChange += informationUI.UpdatePosition;
            agentMonitor.onMaxHealthChange += informationUI.SetMaxHealth;
            agentMonitor.onHealthChange += informationUI.SetCurrentHealth;
            agentMonitor.onDeath += informationUI.OnDeath;
            informationUI.infoText.GetComponent<Text>().text = agentMonitor.name;
            informationUI.SetMaxHealth(agentMonitor.GetMaxHealth());
            informationUI.SetCurrentHealth(agentMonitor.GetCurrentHealth());
            informationUI.UpdatePosition(agentMonitor.transform.position);
            return informationUI;
        }
        
        private void Start()
        {
                
        }

        public void SetMaxHealth(int value)
        {
            this.maxHealth = value;
        }

        public void SetCurrentHealth(int value)
        {
            this.currentHealth = value;
            float rate = (1f * this.currentHealth / this.maxHealth);
            int progress = Convert.ToInt32(rate * this.backImage.sizeDelta.x);
            this.frontImage.sizeDelta = new Vector2(progress,this.frontImage.sizeDelta.y);
        }

        private void UpdatePosition(Vector3 worldPosition)
        {
            var screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main,worldPosition);
            Vector2 localposition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parent,screenPosition,Camera.main, out localposition);
            transform.localPosition = localposition+offset;
        }

        private void OnDeath()
        {
            _agentMonitor.onPositionChange -= this.UpdatePosition;
            _agentMonitor.onDeath -= this.OnDeath;
            _agentMonitor.onHealthChange -= this.SetCurrentHealth;
            _agentMonitor.onMaxHealthChange -= this.SetMaxHealth;
            transform.DOScale(0f, 0.5f).OnComplete(() => { Destroy(gameObject); });

        }
    }
}