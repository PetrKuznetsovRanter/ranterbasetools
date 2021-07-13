using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RanterTools.Movement
{
    public delegate void TargetPointAchievedDelegate();
    /// <summary>
    /// Interface for movement object to target
    /// </summary>
    public interface ITargetMovement
    {
        #region Events
        /// <summary>
        /// Target point achieved event
        /// </summary>
        event TargetPointAchievedDelegate TargetPointAchievedEvent;
        #endregion Events
        #region Parameters  
        float Speed { get; set; }
        /// <summary>
        ///  Get/set a dynamic target for moving an object
        /// </summary>
        /// <value>Static target</value>
        Transform DynamicTarget { get; set; }
        /// <summary>
        ///  Get/set a static target for moving an object
        /// </summary>
        /// <value>Static target</value>
        Vector3 StaticTarget { get; set; }
        #endregion Parameters 
    }

}