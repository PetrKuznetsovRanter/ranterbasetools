using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RanterTools.Base
{

    /// <summary>
    /// Default healing dealer
    /// </summary>
    public class HealingDealer : MonoBehaviour, IHealingDealer
    {
        /// <summary>
        /// Healing dealed event
        /// </summary>  
        public event DamageReceivedDelegate HealingDealedEvent;
        
        /// <summary>
        /// Amount of healing
        /// </summary>
        [SerializeField]
        [Range(0, float.MaxValue)]
        float amountOfHealing;
        
        /// <summary>
        /// Amount of healing
        /// </summary>
        /// <value>Amount of heling</value>
        public float AmountOfHealing { get { return amountOfHealing; } set { amountOfHealing = Mathf.Clamp(value, 0, float.MaxValue); } }
        
        /// <summary>
        /// Heal a damage receiver
        /// </summary>
        /// <param name="damageReceiver">Damage receiver</param>
        public void Heal(IDamageReceiver damageReceiver)
        {
            if (damageReceiver == null)
            {
                Debug.LogError("Damage receiver is null");
                return;
            }
            damageReceiver.Heal(AmountOfHealing);
            if (HealingDealedEvent == null) HealingDealedEvent(AmountOfHealing);
        }
    }


}