using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RanterTools.Base
{
    ///<summary>
    /// Don't destroy on load component
    ///</summary>
    [DisallowMultipleComponent]
    public class DontDestroyOnLoad : MonoBehaviour
    {
        /// <summary>
        /// Don't destroy on load flags.
        /// </summary>
        [SerializeField]
        DontDestroyOnLoadFlags flags = DontDestroyOnLoadFlags.MoveToCurrentSceneAfterRemoveComponent;
        
        bool ApplicationQuited = false;
        
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        /// <summary>
        /// Callback sent to all game objects before the application is quit.
        /// </summary>
        void OnApplicationQuit()
        {
            ApplicationQuited = true;
        }

        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        void OnDestroy()
        {
            if (ApplicationQuited) return;
            if ((flags & DontDestroyOnLoadFlags.MoveToCurrentSceneAfterRemoveComponent) != 0)
            {
                SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
            }
            else if ((flags & DontDestroyOnLoadFlags.DeleteGameObjectAfterRemoveComponent) != 0)
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Don't destroy on load flags enumeration.
    /// </summary>
    public enum DontDestroyOnLoadFlags
    {
        MoveToCurrentSceneAfterRemoveComponent = (1 << 1),
        DeleteGameObjectAfterRemoveComponent = (1 << 2),
    }

}
