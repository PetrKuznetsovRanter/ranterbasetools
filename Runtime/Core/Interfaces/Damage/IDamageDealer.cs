using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RanterTools.Base
{
    /// <summary>
    /// Damage dealer interface
    /// </summary>
    public interface IDamageDealer
    {
        /// <summary>
        /// Damage dealed event
        /// </summary>  
        event DamageReceivedDelegate DamageDealedEvent;
        
        /// <summary>
        /// Damage or amount
        /// </summary>
        /// <value>Damage or amount</value>
        float Damage { get; set; }
        
        /// <summary>
        /// Deal damage
        /// </summary>
        /// <param name="receiver">Damage receiver</param>
        void DealDamage(IDamageReceiver damageReceiver);
    }
}