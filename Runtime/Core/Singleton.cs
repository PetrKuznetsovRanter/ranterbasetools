using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace RanterTools.Base
{
    /// <summary>
    /// Singleton base class
    /// </summary>
    /// <typeparam name="T">Type of singleton</typeparam>
    public abstract class Singleton<T> where T : class, new()
    {
        #region Global State
        /// <summary>
        /// Instance of singleton
        /// </summary>
        T instance = null;

        /// <summary>
        /// Instance of singleton property
        /// </summary>
        /// <value>Instance of singleton</value>
        public T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }
                return instance;
            }
        }
        #endregion Global State
    }

}