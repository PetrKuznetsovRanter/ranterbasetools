using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RanterTools.Base
{

    /// <summary>
    /// Default damage receiver
    /// </summary>
    public class DamageReceiver : MonoBehaviour, IDamageReceiver
    {
        #region Events 
        #region IDamageReceiver
        /// <summary>
        /// Damage received event
        /// </summary>
        public event DamageReceivedDelegate DamageReceivedEvent;
        /// <summary>
        /// Healing received event
        /// </summary>
        public event DamageReceivedDelegate HealingReceivedEvent;
        /// <summary>
        /// Death event
        /// </summary>
        public event DamageReceivedDelegate DeathEvent;
        /// <summary>
        /// Resurrect event
        /// </summary>
        public event DamageReceivedDelegate ResurrectEvent;
        #endregion IDamageReceiver
        #endregion Events

        #region Parameters
        /// <summary>
        /// Initiated health point
        /// </summary>
        [SerializeField]
        [Range(0, float.MaxValue)]
        float initiatedHealthPoint;
        #region IDamageReceiver
        /// <summary>
        /// Initiated health point
        /// </summary>
        /// <value>Initiated health point</value>
        public float InitiatedHealthPoint { get { return initiatedHealthPoint; } set { initiatedHealthPoint = Mathf.Clamp(initiatedHealthPoint, 0, float.MaxValue); } }
        #endregion IDamageReceiver
        #endregion Parameters

        #region State
        /// <summary>
        /// Current health point
        /// </summary>
        float healthPoint;
        /// <summary>
        /// Current health point
        /// </summary>
        /// <value>Current health point</value>
        public float HealthPoint { get { return healthPoint; } }
        #endregion State

        #region Methods
        #region IDamageReceiver
        /// <summary>
        /// Restore some amount hp.
        /// </summary>
        /// <param name="amountOfHealing"></param>
        public void Heal(float amountOfHealing)
        {
            healthPoint += amountOfHealing;
            if (healthPoint > InitiatedHealthPoint) healthPoint = InitiatedHealthPoint;
            if (HealingReceivedEvent != null) HealingReceivedEvent(amountOfHealing);
        }
        /// <summary>
        /// Restore all HP.
        /// </summary>
        public void Resurrect()
        {
            healthPoint = InitiatedHealthPoint;
            if (ResurrectEvent != null) ResurrectEvent(healthPoint);
        }
        /// <summary>
        /// Received damage
        /// </summary>
        /// <param name="damage">Amount of damage</param>
        public void Damage(float damage)
        {
            healthPoint -= damage;
            if (healthPoint <= 0)
            {
                if (DeathEvent != null) DeathEvent(damage);
                healthPoint = 0;
            }
            else
            {
                if (DamageReceivedEvent != null) DamageReceivedEvent(damage);
            }
        }
        #endregion IDamageReceiver
        #endregion Methods

        #region Unity
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            Resurrect();
        }
        #endregion Unity

    }
}