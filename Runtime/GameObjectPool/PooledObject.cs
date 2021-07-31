using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RanterTools.Base;

public class PooledObject : MonoBehaviour, IPooledObject
{
    /// <summary>
    /// Pooled object start event
    /// </summary>
    public event System.Action PooledObjectStartEvent;
    /// <summary>
    /// Pooled object destroy event
    /// </summary>
    public event System.Action PooledObjectDestroyEvent;
    
    /// <summary>
    /// Parent prefab used for creation this instance.
    /// </summary>
    Object parentPrefab;
    /// <summary>
    /// Parent prefab used for creation this instance.
    /// </summary>
    /// <value>Parent prefab used for creation this instance</value>
    public Object ParentPrefab { get { return parentPrefab; } set { if (parentPrefab == null) parentPrefab = value; } }


    /// <summary>
    /// Parent pool container used for creation this instance
    /// </summary>
    /// <value>Parent pool container used for creation this instance</value>
    public PoolContainer ParentPoolContainer { get; set; }
    
    /// <summary>
    ///  Custom handler for start pooled game object
    /// </summary>
    public void StartPooledObject()
    {
        if (PooledObjectStartEvent != null) PooledObjectStartEvent();
    }
    /// <summary>
    /// Custom handler for destroy pooled game object
    /// </summary>
    public void DestroyPooledObject()
    {
        if (PooledObjectDestroyEvent != null) PooledObjectDestroyEvent();
    }

    void PoolContainerDestroyed(PoolContainer poolContainer)
    {
        if (ParentPoolContainer == null || poolContainer == ParentPoolContainer) DestroyImmediate(gameObject);
    }
    
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        PoolContainer.OnPoolContainerDestroyed += PoolContainerDestroyed;
    }
    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        PoolContainer.OnPoolContainerDestroyed -= PoolContainerDestroyed;
    }
}
