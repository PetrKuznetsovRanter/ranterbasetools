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
        #region Global State
        public static event System.Action<PoolContainer> OnPoolContainerDestroyed;
        #endregion Global State
        #region State
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
        #endregion State
        #region Unity
        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        void OnDestroy()
        {
            if (OnPoolContainerDestroyed != null) OnPoolContainerDestroyed(this);
        }
        #endregion Unity
    }


}
