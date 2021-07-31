using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RanterTools.Base
{

    /// <summary>
    /// Container that contained pooled gameObjects.
    /// </summary>
    public class PoolContainer : MonoBehaviour
    {
        public static event System.Action<PoolContainer> OnPoolContainerDestroyed;
        
        /// <summary>
        ///  Origin for instance or destroying pooled gameObjects.
        /// </summary>
        Object parentOrigin;
        /// <summary>
        /// Origin for instance or destroying pooled gameObjects property.
        /// </summary>
        /// <value> Origin for instance or destroying pooled gameObjects.</value>
        public Object ParentOrigin
        {
            get { return parentOrigin; }
            set { parentOrigin = value; }
        }
        
        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        void OnDestroy()
        {
            if (OnPoolContainerDestroyed != null) OnPoolContainerDestroyed(this);
        }
        
    }


}
