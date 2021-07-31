using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RanterTools.Base
{
    /// <summary>
    /// Default damage dealer
    /// </summary>
    public class DamageDealer : MonoBehaviour, IDamageDealer
    {
        /// <summary>
        /// Damage dealed event
        /// </summary>  
        public event DamageReceivedDelegate DamageDealedEvent;
        
        [SerializeField]
        [Range(0, float.MaxValue)]
        float damage = 1;
        
        /// <summary>
        /// Damage or amount
        /// </summary>
        /// <value>Damage or amount</value>
        public float Damage { get { return damage; } set { damage = Mathf.Clamp(value, 0, float.MaxValue); } }
        
        /// <summary>
        /// Deal damage
        /// </summary>
        /// <param name="receiver">Damage receiver</param>
        public void DealDamage(IDamageReceiver damageReceiver)
        {
            if (damageReceiver == null)
            {
                Debug.LogError("Damage receiver is null");
                return;
            }
            damageReceiver.Damage(Damage);
            if (DamageDealedEvent != null) DamageDealedEvent(Damage);
        }
    }

}
