using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RanterTools.Base
{

    public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region Global State
        /// <summary>
        /// Flags describing the behavior of singleton
        /// </summary>
        public static SingletonBehaviourFlags SingletonBehaviourFlags = SingletonBehaviourFlags.All;
        protected static T instance = null;
        /// <summary>
        /// Instance of singleton
        /// </summary>
        /// <value>Instance of singleton</value>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    List<T> monoBehaviours = new List<T>();
                    var roots = new List<T>(Resources.FindObjectsOfTypeAll<T>());
#if UNITY_EDITOR
                    foreach (var ob in roots)
                    {
                        if (!UnityEditor.EditorUtility.IsPersistent(ob))
                        {
                            monoBehaviours.Add(ob);
                        }
                    }
#else
                    monoBehaviours.AddRange(roots);
#endif
                    if (monoBehaviours.Count == 0)
                    {
                        ToolsDebug.Log($"Can't find instance of type {typeof(T)}. Create new instance.");
                        CreateNewInstance();
                        CheckDontDestroyOnLoad(instance);
                    }
                    else if (monoBehaviours.Count == 1)
                    {
                        instance = monoBehaviours[0];
                        CheckDontDestroyOnLoad(instance);
                        if ((SingletonBehaviourFlags & SingletonBehaviourFlags.DontDestroyOnLoadOnNew) != 0)
                            if (instance.gameObject.GetComponent<DontDestroyOnLoad>() == null)
                                instance.gameObject.AddComponent<DontDestroyOnLoad>();
                    }
                    else
                    {
                        ToolsDebug.LogWarning("Singleton " + typeof(T) + " have few instance on scene. First entry received.");
                        instance = monoBehaviours[0];
                        CheckDontDestroyOnLoad(instance);
                        if ((SingletonBehaviourFlags & SingletonBehaviourFlags.DestroyExcess) != 0)
                        {
                            for (int i = 1; i < monoBehaviours.Count; i++)
                            {
                                DestroyImmediate(monoBehaviours[i]);
                            }
                        }
                    }
                }
                return instance;
            }
        }

        static void CreateNewInstance()
        {
            GameObject gameObjectTmp = new GameObject();
            gameObjectTmp.name = $"{typeof(T).Name} (Singleton)";
            instance = gameObjectTmp.AddComponent<T>();
        }
        static void CheckDontDestroyOnLoad(T i)
        {
            if (i == null) throw new System.Exception($"Instance of type {typeof(T)} is null in internal method CheckDontDestroyOnLoad.");
            var dontDestroyOnLoad = i.gameObject.GetComponent<DontDestroyOnLoad>();
            if ((SingletonBehaviourFlags & SingletonBehaviourFlags.DontDestroyOnLoadOnNew) != 0)
            {
                if (dontDestroyOnLoad == null) i.gameObject.AddComponent<DontDestroyOnLoad>();
            }
            else
            {
                if (dontDestroyOnLoad != null) DestroyImmediate(dontDestroyOnLoad);
            }
        }
        #endregion Global State
    }

    /// <summary>
    /// Flags describing the behavior of singleton
    /// </summary>
    public enum SingletonBehaviourFlags
    {
        None = 0, DontDestroyOnLoadOnNew = 1, DestroyExcess = 2, All = ~0
    }

}
