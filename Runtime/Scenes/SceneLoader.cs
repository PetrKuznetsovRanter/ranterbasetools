using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace RanterTools.Scenes
{

    /// <summary>
    /// Scene loader component for load/unload scene.
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        /// <summary>
        /// Reference to scene.
        /// </summary>
        [Header("Scene Loader")]
        [SerializeField]
        SceneRef scene;
        /// <summary>
        /// Load scene mode.
        /// </summary>
        [SerializeField]
        LoadSceneMode loadSceneMode;
        
        /// <summary>
        /// Load scene.
        /// </summary>
        public void LoadScene()
        {
            SceneManager.LoadScene(scene.Name, loadSceneMode);
        }
        /// <summary>
        /// Load scene async.
        /// </summary>
        public void LoadSceneAsync()
        {
            SceneManager.LoadSceneAsync(scene.Name, loadSceneMode);
        }
        /// <summary>
        /// Unload scene.
        /// </summary>
        public void UnloadScene()
        {
            if (loadSceneMode == LoadSceneMode.Additive) SceneManager.UnloadSceneAsync(scene.Name);
            else
            {
                if (SceneManager.GetActiveScene().name == scene.Name)
                {
                    if (SceneManager.sceneCount >= 2)
                    {
                        Scene sceneTmp;
                        for (int i = 0; i < SceneManager.sceneCount; i++)
                        {
                            sceneTmp = SceneManager.GetSceneAt(i);
                            if (sceneTmp.name != scene.Name)
                            {
                                SceneManager.SetActiveScene(sceneTmp);
                                SceneManager.UnloadSceneAsync(scene.Name);
                                return;
                            }
                        }
                    }
                    else Debug.LogError("Can't unload once scene.");
                }
                else SceneManager.UnloadSceneAsync(scene.Name);
            }
        }
    }




}
