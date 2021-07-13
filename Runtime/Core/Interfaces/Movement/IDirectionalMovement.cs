using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RanterTools.Movement
{
    /// <summary>
    /// Interface for directional movement of object
    /// </summary>
    public interface IDirectionalMovement
    {
        /// <summary>
        /// Scalar value of velocity property
        /// </summary>
        /// <value>Scalar value of velocity</value>
        float Speed { get; set; }
        
        /// <summary>
        /// Velocity direction property
        /// </summary>
        /// <value>Velocity direction</value>
        Vector3 Direction { get; set; }
    }
}