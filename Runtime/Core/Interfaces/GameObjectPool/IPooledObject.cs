using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RanterTools.Base
{
    /// <summary>
    /// Interface for pooled game object
    /// </summary>
    public interface IPooledObject
    {
        /// <summary>
        /// Pooled object start event
        /// </summary>
        event System.Action PooledObjectStartEvent;
        
        /// <summary>
        /// Pooled object destroy event
        /// </summary>
        event System.Action PooledObjectDestroyEvent;
        
        /// <summary>
        /// Parent prefab used for creation this instance
        /// </summary>
        /// <value>Parent prefabe used for creation this instance</value>
        Object ParentPrefab { get; set; }

        /// <summary>
        /// Parent pool container used for creation this instance
        /// </summary>
        /// <value>Parent pool container used for creation this instance</value>
        PoolContainer ParentPoolContainer { get; set; }

        /// <summary>
        ///  Custom handler for start pooled game object
        /// </summary>
        void StartPooledObject();
        
        /// <summary>
        /// Custom handler for destroy pooled game object
        /// </summary>
        void DestroyPooledObject();
    }
}