using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RanterTools.Base
{
    public class GlobalUpdate : SingletonBehaviour<GlobalUpdate>
    {
        public delegate void GlobalUpdateDelegate();
        /// <summary>
        /// Events for invoke update from one instance against multiply. Because invoke update is slow for deferent scripts. 
        /// </summary>
        public static event GlobalUpdateDelegate GlobalUpdateEvent;
        
        [RuntimeInitializeOnLoadMethod]
        public static void CheckGlobalUpdateInstance()
        {
            if (Instance == null)
            {
                Debug.LogError("Can't create global update object.");
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Instance == this)
            {
                if (GlobalUpdateEvent != null) GlobalUpdateEvent();
            }
        }
    }

}