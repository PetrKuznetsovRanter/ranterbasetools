using UnityEngine;
using System.Collections.Generic;


namespace RanterTools.Base
{
    /// <summary>
    /// GameObject Extentions
    /// </summary>
    public static class GameObjectExtentions
    {
        /// <summary>
        /// Pool of GameObjects.
        /// </summary>
        /// <typeparam name="GameObject">Prefab or GameObject which describe their pool. </typeparam>
        /// <typeparam name="GameObjectPool">Pool of instances of this prefab or GameObjects.</typeparam>
        /// <returns>Dictionary for rapid access of instances.</returns>
        static Dictionary<GameObject, GameObjectPool> pool = new Dictionary<GameObject, GameObjectPool>();
        public static GameObject rootObjectForPools;
        
        /// <summary>
        /// Initiate game objects pool with size.
        /// </summary>
        /// <param name="prefab">Prefab or gameObject for creating objects on pool.</param>
        /// <param name="size">Size of pool.</param>
        public static void InitiateGameObjectPool(this GameObject prefab, int size = 1000)
        {
            if (rootObjectForPools == null)
            {
                rootObjectForPools = new GameObject();
                rootObjectForPools.name = "Pools of objects";
            }

            PoolContainer poolContainer;
            GameObject container;
            if (pool.ContainsKey(prefab))
            {
                if (pool[prefab].poolContainer == null)
                {
                    container = new GameObject();
                    container.transform.parent = rootObjectForPools.transform;
                    container.name = "Container " + prefab.name;
                    poolContainer  = container.AddComponent<PoolContainer>();
                    poolContainer.ParentOrigin = prefab;
                    pool[prefab].poolContainer = poolContainer;
                }
                pool[prefab].ResizePool(size);
                return;
            }
            container = new GameObject();
            container.transform.parent = rootObjectForPools.transform;
            container.name = "Container " + prefab.name;
            poolContainer = container.AddComponent<PoolContainer>();
            poolContainer.ParentOrigin = prefab;
            GameObjectPool poolTmp = new GameObjectPool();
            poolTmp.poolContainer = poolContainer;
            poolTmp.ResizePool(prefab, size);
            pool[prefab] = poolTmp;
        }

        /// <summary>
        /// Destroy game objects pool of prefab.
        /// </summary>
        /// <param name="prefab">Prefab or gameObject for destroy game objects pool of this.</param>
        public static void DestroyGameObjectPool(this GameObject prefab)
        {
            if (rootObjectForPools == null) return;
            if (!pool.ContainsKey(prefab)) return;
            pool[prefab].DestroyAll();
            pool.Remove(prefab);
        }
        
        /// <summary>
        /// Special instansiate method for pooled GameObjects.
        /// </summary>
        public static GameObject InstancePooledObject(this GameObject prefab)
        {
            if (pool.ContainsKey(prefab))
            {
                return pool[prefab].Instance();
            }
            else
            {
                InitiateGameObjectPool(prefab);
                return pool[prefab].Instance();
            }
        }

        /// <summary>
        /// Special destroy method for pooled GameObjects.
        /// </summary>
        public static void DestroyPooledObject(this GameObject prefab, GameObject gameObject)
        {

            if (pool.ContainsKey(prefab))
            {
                pool[prefab].Destroy(gameObject);
            }
        }

        /// <summary>
        /// Get component or create it if it not exist
        /// </summary>
        public static T GetOrCreateComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = default(T);
            component = gameObject.GetComponent<T>();
            if (component == null) component = gameObject.AddComponent<T>();
            return component;
        }
        /// <summary>
        /// Get component or create default it if it not exist
        /// </summary>
        public static I GetOrCreateComponent<I, T>(this GameObject gameObject) where T : Component, I
        {
            I component = default(I);
            component = gameObject.GetComponent<I>();
            if (component == null) component = (I)gameObject.AddComponent<T>();
            return component;
        }
    }


    [System.Serializable]
    class GameObjectPool
    {
        /// <summary>
        /// PoolContainer pool container
        /// </summary>
        public PoolContainer poolContainer;
        
        /// <summary>
        /// Parent prefab for creation instances.
        /// </summary>
        GameObject prefab;
        
        /// <summary>
        /// Dictionary for available gameObjects in pool.
        /// </summary>
        /// <typeparam name="GameObject">Key type.</typeparam>
        /// <typeparam name="GameObject">Value type.</typeparam>
        /// <returns>GameObject.</returns>
        Dictionary<GameObject, GameObject> available = new Dictionary<GameObject, GameObject>();
        
        /// <summary>
        /// Dictionary for inaccessible gameObjects in pool.
        /// </summary>
        /// <typeparam name="GameObject">Key type.</typeparam>
        /// <typeparam name="GameObject">Value type.</typeparam>
        /// <returns>GameObject.</returns>
        Dictionary<GameObject, GameObject> inaccessible = new Dictionary<GameObject, GameObject>();
        
        /// <summary>
        /// Current pool size.
        /// </summary>
        int currentSize = 10;
        
        /// <summary>
        /// Resize gameobject pool.
        /// </summary>
        /// <param name="prefab">Parent prefab for creation instances.</param>
        /// <param name="size">Pool size.</param>
        public void ResizePool(GameObject prefab, int size)
        {
            if (size <= 2) return;
            if (size <= available.Count)
            {
                GameObject temp;
                var enumerator = available.GetEnumerator();
                enumerator.MoveNext();
                for (int i = 0; i < (available.Count - size); i++)
                {
                    temp = enumerator.Current.Value;
                    available.Remove(temp);
                    GameObject.DestroyImmediate(temp);
                }
            }
            else
            {
                GameObject temp;
                IPooledObject interfacePooledObject = null;
                for (int i = 0; i < (size - available.Count); i++)
                {
                    temp = GameObject.Instantiate(prefab);
                    interfacePooledObject = temp.GetComponent<IPooledObject>();
                    if (interfacePooledObject == null) interfacePooledObject = temp.AddComponent<PooledObject>();
                    interfacePooledObject.ParentPrefab = prefab;
                    interfacePooledObject.ParentPoolContainer = poolContainer;
                    temp.transform.parent = poolContainer.transform;
                    temp.SetActive(false);
                    available[temp] = temp;
                }
            }
            this.prefab = prefab;
            currentSize = size;
        }
        
        /// <summary>
        /// Resize gameobject pool.
        /// </summary>
        /// <param name="size">Pool size.</param>
        public void ResizePool(int size)
        {
            ResizePool(prefab, size);
        }

        /// <summary>
        /// Get instance from gameObject pool.
        /// </summary>
        /// <returns></returns>
        public GameObject Instance()
        {
            if (available.Count != 0)
            {
                IPooledObject interfacePooledObject = null;
                var enumerator = available.GetEnumerator();
                enumerator.MoveNext();
                GameObject temp = enumerator.Current.Key;
                interfacePooledObject = temp.GetComponent<IPooledObject>();
                if (interfacePooledObject == null) interfacePooledObject = temp.AddComponent<PooledObject>();
                available.Remove(temp);
                inaccessible[temp] = temp;
                temp.SetActive(true);
                interfacePooledObject.StartPooledObject();
                return temp;
            }
            else
            {
                throw (new GameObjectPoolException());
            }
        }
        
        /// <summary>
        /// Destroy instance of pooled gameObject.
        /// </summary>
        /// <param name="gameObject">Deleting gameObject.</param>
        public void Destroy(GameObject gameObject)
        {
            if (inaccessible.ContainsKey(gameObject))
            {
                IPooledObject interfacePooledObject = null;
                interfacePooledObject = gameObject.GetComponent<IPooledObject>();
                if (interfacePooledObject == null) interfacePooledObject = gameObject.AddComponent<PooledObject>();
                interfacePooledObject.DestroyPooledObject();
                inaccessible.Remove(gameObject);
                available[gameObject] = gameObject;
                gameObject.SetActive(false);
                gameObject.transform.parent = poolContainer.transform;
            }
        }
        
        /// <summary>
        /// Destroy all gameObjects in this pool.
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
                    if (interfacePooledObject == null) interfacePooledObject = iterator.Current.Value.AddComponent<PooledObject>();
                    interfacePooledObject.DestroyPooledObject();
                    available[iterator.Current.Value] = iterator.Current.Value;
                    iterator.Current.Value.SetActive(false);
                    iterator.Current.Value.transform.parent = poolContainer.transform;
                }
                else
                {
                    if (iterator.Current.Value != null) GameObject.DestroyImmediate(iterator.Current.Value);
                }
            }
            inaccessible.Clear();
        }
    }
    
    /// <summary>
    /// Exception if pool empty.
    /// </summary>
    public class GameObjectPoolException : System.Exception
    {
        public override string Message { get { return "Try get instance from empty pool."; } }
    }
}
