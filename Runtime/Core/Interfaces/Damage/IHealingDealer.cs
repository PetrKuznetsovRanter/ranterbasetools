using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RanterTools.Base
{
    /// <summary>
    /// Healing dealer interface
    /// </summary>
    public interface IHealingDealer
    {
        /// <summary>
        /// Healing dealed event
        /// </summary>  
        event DamageReceivedDelegate HealingDealedEvent;
        
        /// <summary>
        /// Amount of healing
        /// </summary>
        /// <value>Amount of heling</value>
        float AmountOfHealing { get; set; }
        
        /// <summary>
        /// Heal a damage receiver
        /// </summary>
        /// <param name="damageReceiver">Damage receiver</param>
        void Heal(IDamageReceiver damageReceiver);
    }
}