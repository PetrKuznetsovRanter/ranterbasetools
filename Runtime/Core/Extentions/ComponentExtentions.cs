using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RanterTools.Base
{
    /// <summary>
    /// Component Extentions
    /// </summary>
    public static class ComponentExtentions
    {

        #region Global State
        #region ComponentPool
        /// <summary>
        /// Pool of Components.
        /// </summary>
        /// <typeparam name="Component">Component which describe their pool. </typeparam>
        /// <typeparam name="ComponentPool">Pool of instances of this Components.</typeparam>
        /// <returns>Dictionary for rapid access of instances.</returns>
        static Dictionary<Component, ComponentPool> pool = new Dictionary<Component, ComponentPool>();
        public static GameObject rootComponentForPools;
        #endregion ComponentPool
        #endregion Global State

        #region Global Methods
        #region ComponentPool
        /// <summary>
        /// Initiate Components pool with size.
        /// </summary>
        /// <param name="origin">Origin of Component for creating Components on pool.</param>
        /// <param name="size">Size of pool.</param>
        public static void InitiateComponentPool<T>(this T origin, int size = 1000) where T:Component
        {
            if (rootComponentForPools == null)
            {
                rootComponentForPools = new GameObject();
                rootComponentForPools.name = "Pools of Components";
            }
            PoolContainer poolContainer;
            GameObject container;
            if (pool.ContainsKey(origin))
            {
                if (pool[origin].poolContainer == null)
                {
                    container = new GameObject();
                    container.transform.parent = rootComponentForPools.transform;
                    container.name = "Container " + origin.name;
                    poolContainer  = container.AddComponent<PoolContainer>();
                    poolContainer.ParentOrigin = origin;
                    pool[origin].poolContainer = poolContainer;
                }
                pool[origin].ResizePool(size);
                return;
            }
            container = new GameObject();
            container.transform.parent = rootComponentForPools.transform;
            container.name = "Container " + origin.name;
            poolContainer = container.AddComponent<PoolContainer>();
            poolContainer.ParentOrigin = origin;
            ComponentPool poolTmp = new ComponentPool();;
            poolTmp.poolContainer = poolContainer;
            poolTmp.ResizePool(origin, size);
            pool[origin] = poolTmp;
        }

        /// <summary>
        /// Destroy Components pool of origin.
        /// </summary>
        /// <param name="origin">Origin of Component for destroy game Components pool of this.</param>
        public static void DestroyComponentPool<T>(this T origin)where T:Component
        {
            if (rootComponentForPools == null) return;
            if (!pool.ContainsKey(origin)) return;
            pool[origin].DestroyAll();
            pool.Remove(origin);
        }


        /// <summary>
        /// Special instansiate method for pooled Components.
        /// </summary>
        public static T InstancePooledComponent<T>(this T origin)where T:Component
        {
            if (pool.ContainsKey(origin))
            {
                return pool[origin].Instance()as T;
            }
            else
            {
                InitiateComponentPool(origin);
                return pool[origin].Instance() as T;
            }
        }

        /// <summary>
        /// Special destroy method for pooled Components.
        /// </summary>
        public static void DestroyPooledComponent<T>(this T origin, T Component)where T:Component
        {
            if (pool.ContainsKey(origin))
            {
                pool[origin].Destroy(Component);
            }
        }
        #endregion ComponentPool


        #endregion Unity
    }


    [System.Serializable]
    class ComponentPool
    {
        /// <summary>
        /// GameObject pool container
        /// </summary>
        public PoolContainer poolContainer;
        
        /// <summary>
        /// Parent origin for creation instances.
        /// </summary>
        Component origin;
        
        /// <summary>
        /// Dictionary for available Components in pool.
        /// </summary>
        /// <typeparam name="Component">Key type.</typeparam>
        /// <typeparam name="Component">Value type.</typeparam>
        /// <returns>Component.</returns>
        Dictionary<Component, Component> available = new Dictionary<Component, Component>();
        
        /// <summary>
        /// Dictionary for inaccessible Components in pool.
        /// </summary>
        /// <typeparam name="Component">Key type.</typeparam>
        /// <typeparam name="Component">Value type.</typeparam>
        /// <returns>Component.</returns>
        Dictionary<Component, Component> inaccessible = new Dictionary<Component, Component>();
        
        /// <summary>
        /// Current pool size.
        /// </summary>
        int currentSize = 10;
        
        /// <summary>
        /// Resize Component pool.
        /// </summary>
        /// <param name="origin">Parent origin for creation instances.</param>
        /// <param name="size">Pool size.</param>
        public void ResizePool(Component origin, int size)
        {
            Debug.Log($"Pool new size {size} old size {available.Count+inaccessible.Count}");
            if (size <= 2) return;
            if (size <= available.Count)
            {
                Component temp;
                var enumerator = available.GetEnumerator();
                enumerator.MoveNext();
                for (int i = 0; i < (available.Count - size); i++)
                {
                    temp = enumerator.Current.Value;
                    available.Remove(temp);
                    Component.DestroyImmediate(temp);
                }
            }
            else
            {
                Component temp;
                IPooledObject interfacePooledObject = null;
                for (int i = 0; i < (size - available.Count); i++)
                {
                    temp = Component.Instantiate(origin);
                    interfacePooledObject = temp.GetComponent<IPooledObject>();
                    if (interfacePooledObject == null) interfacePooledObject = temp.gameObject.AddComponent<PooledObject>();
                    interfacePooledObject.ParentPrefab = origin;
                    interfacePooledObject.ParentPoolContainer = poolContainer;
                    temp.transform.SetParent(poolContainer.transform);
                    temp.gameObject.SetActive(false);
                    available[temp] = temp;
                }
            }
            this.origin = origin;
            currentSize = size;
        }
        
        /// <summary>
        /// Resize Component pool.
        /// </summary>
        /// <param name="size">Pool size.</param>
        public void ResizePool(int size)
        {
            ResizePool(origin, size);
        }

        /// <summary>
        /// Get instance from Component pool.
        /// </summary>
        /// <returns></returns>
        public Component Instance()
        {
            if (available.Count == 0)
            {
                int size = inaccessible.Count;
                if (size == 0) size = 1000;
                else size *= 2;
                ResizePool(size);
            }
            if (available.Count != 0)
            {
                IPooledObject interfacePooledObject = null;
                var enumerator = available.GetEnumerator();
                enumerator.MoveNext();
                Component temp = enumerator.Current.Key;
                interfacePooledObject = temp.GetComponent<IPooledObject>();
                if (interfacePooledObject == null) interfacePooledObject = temp.gameObject.AddComponent<PooledObject>();
                available.Remove(temp);
                inaccessible[temp] = temp;
                temp.gameObject.SetActive(true);
                interfacePooledObject.StartPooledObject();
                return temp;
            }
            else
            {
                throw (new ComponentPoolException());
            }
        }
        
        /// <summary>
        /// Destroy instance of pooled Component.
        /// </summary>
        /// <param name="Component">Deleting Component.</param>
        public void Destroy(Component Component)
        {
            if (inaccessible == null || Component==null) return;
            if (inaccessible.ContainsKey(Component))
            {
                IPooledObject interfacePooledObject = null;
                interfacePooledObject = Component.GetComponent<IPooledObject>();
                if (interfacePooledObject == null) interfacePooledObject = Component.gameObject.AddComponent<PooledObject>();
                interfacePooledObject.DestroyPooledObject();
                inaccessible.Remove(Component);
                available[Component] = Component;
                Component.transform.SetParent(poolContainer.transform);
                Component.gameObject.SetActive(false);
                
            }
        }
        
        /// <summary>
        /// Destroy all Components in this pool.
        /// </summary>
        public void DestroyAll(bool justInited = false)
        {
            var iterator = inaccessible.GetEnumerator();
            while (iterator.MoveNext())
            {
                if (!justInited)
                {
                    IPooledObject interfacePooledObject = null;
                    interfacePooledObject = iterator.Current.Value.GetComponent<IPooledObject>();
                    if (interfacePooledObject == null) interfacePooledObject = iterator.Current.Value.gameObject.AddComponent<PooledObject>();
                    interfacePooledObject.DestroyPooledObject();
                    available[iterator.Current.Value] = iterator.Current.Value;
                    iterator.Current.Value.gameObject.SetActive(false);
                    iterator.Current.Value.transform.parent = poolContainer.transform;
                }
                else
                {
                    Debug.Log($"Value {iterator.Current.Value}");
                    if (iterator.Current.Value != null) Component.DestroyImmediate(iterator.Current.Value);
                }
            }
            inaccessible.Clear();
        }
    }
    
    /// <summary>
    /// Exception if pool empty.
    /// </summary>
    public class ComponentPoolException : System.Exception
    {
        public override string Message { get { return "Try get instance from empty pool."; } }
    }

}
