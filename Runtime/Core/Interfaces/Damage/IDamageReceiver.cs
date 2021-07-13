using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RanterTools.Base
{
    public delegate void DamageReceivedDelegate(float damage);

    /// <summary>
    /// Damage receiver interface
    /// </summary>
    public interface IDamageReceiver
    {
        /// <summary>
        /// Damage received event
        /// </summary>
        event DamageReceivedDelegate DamageReceivedEvent;
        
        /// <summary>
        /// Healing received event
        /// </summary>
        event DamageReceivedDelegate HealingReceivedEvent;
        
        /// <summary>
        /// Death event
        /// </summary>
        event DamageReceivedDelegate DeathEvent;
        
        /// <summary>
        /// Resurrect event
        /// </summary>
        event DamageReceivedDelegate ResurrectEvent;
        
        /// <summary>
        /// Initiated health point
        /// </summary>
        /// <value>Initiated health point</value>
        float InitiatedHealthPoint { get; set; }
        
        /// <summary>
        /// Current health point
        /// </summary>
        /// <value>Current health point</value>
        float HealthPoint { get; }
        
        /// <summary>
        /// Restore some amount hp.
        /// </summary>
        /// <param name="amountOfHealing"></param>
        void Heal(float amountOfHealing);
        
        /// <summary>
        /// Restore all HP.
        /// </summary>
        void Resurrect();
        
        /// <summary>
        /// Received damage
        /// </summary>
        /// <param name="damage">Amount of damage</param>
        void Damage(float damage);
    }
}