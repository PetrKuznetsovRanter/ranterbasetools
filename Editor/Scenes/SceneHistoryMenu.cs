using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using RanterTools.Scenes;
using RanterTools.Base;
namespace RanterTools.Editor.Scenes
{
    //TODO:
    //Update names for all sceneref if scene change name
    //Update history if scene closed like additive.
    /// <summary>
    /// Scene history menu.
    /// </summary>
    [InitializeOnLoad]
    public class SceneHistoryMenu
    {
        #region Global State
        /// <summary>
        /// Length of history.
        /// </summary>
        const int maxLength = 32;
        /// <summary>
        /// Cursor pointing on current scene.
        /// </summary>
        static int cursor = -1;

        /// <summary>
        /// User load new scene.
        /// </summary>
        static bool userLoadedScene = false;

        /// <summary>
        /// Last active scene.c Need for older unity version.
        /// </summary>
        static string lastActiveScene;

        /// <summary>
        /// Scene history list.
        /// </summary>
        /// <typeparam name="SceneRef">Scene type.</typeparam>
        /// <returns></returns>
        static List<SceneRef> sceneHistoryList = new List<SceneRef>(maxLength);
        #endregion Global State

        #region Global Methods

        /// <summary>
        /// Load next scene in history.
        /// </summary>
        [MenuItem(GlobalNames.MenuRanterTools + "/" + GlobalNames.ScenesHistory + "/" + GlobalNames.LoadNextScene)]
        public static void LoadNextScene()
        {
            ToolsDebug.Log("LoadNextScene " + cursor + " " + sceneHistoryList.Count);
            if (cursor > sceneHistoryList.Count - 2 || cursor == -1) return;
            userLoadedScene = true;
#if UNITY_5_0_OR_NEWER || UNITY_2017_1_OR_NEWER
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
#else
            EditorApplication.SaveCurrentSceneIfUserWantsTo();
#endif
            cursor++;

#if UNITY_5_0_OR_NEWER || UNITY_2017_1_OR_NEWER
            EditorSceneManager.OpenScene(sceneHistoryList[cursor]);
#else
            EditorApplication.OpenScene(sceneHistoryList[cursor]);
#endif
        }

        /// <summary>
        /// Load previous scene in history.
        /// </summary>
        [MenuItem(GlobalNames.MenuRanterTools + "/" + GlobalNames.ScenesHistory + "/" + GlobalNames.LoadPreviousScene)]
        public static void LoadPreviousScene()
        {
            userLoadedScene = true;
            ToolsDebug.Log("LoadPrevScene " + cursor + " " + sceneHistoryList.Count);
            if (cursor <= 0) return;
#if UNITY_5_0_OR_NEWER || UNITY_2017_1_OR_NEWER
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
#else
            EditorApplication.SaveCurrentSceneIfUserWantsTo();
#endif
            cursor--;

#if UNITY_5_0_OR_NEWER || UNITY_2017_1_OR_NEWER
            EditorSceneManager.OpenScene(sceneHistoryList[cursor]);
#else
            EditorApplication.OpenScene(sceneHistoryList[cursor]);
#endif
        }


        [MenuItem(GlobalNames.MenuRanterTools + "/" + GlobalNames.ScenesHistory + "/" + GlobalNames.LoadFirstScene)]
        public static void LoadFirstScene()
        {
            ToolsDebug.Log("LoadFirstScene " + cursor + " " + sceneHistoryList.Count);
            userLoadedScene = true;
#if UNITY_5_0_OR_NEWER || UNITY_2017_1_OR_NEWER
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            if (sceneHistoryList.Count >= maxLength)
            {
                sceneHistoryList.RemoveAt(0);
                sceneHistoryList.Add(new SceneRef(EditorSceneManager.GetActiveScene().path));
            }
            else
            {
                sceneHistoryList.Add(new SceneRef(EditorSceneManager.GetActiveScene().path));
            }
#else
            EditorApplication.SaveCurrentSceneIfUserWantsTo();
            if (sceneHistoryList.Count >= maxLength)
            {
                sceneHistoryList.RemoveAt(0);
                sceneHistoryList.Add(EditorApplication.currentScene);
            }
            else
            {
                sceneHistoryList.Add(EditorApplication.currentScene);
            }
#endif

#if UNITY_5_0_OR_NEWER || UNITY_2017_1_OR_NEWER
            if (EditorSceneManager.sceneCountInBuildSettings != 0)
            {
                if (EditorSceneManager.GetSceneByBuildIndex(0) != null)
                    EditorSceneManager.OpenScene(EditorBuildSettings.scenes[0].path, OpenSceneMode.Single);
                else
                {
                    ToolsDebug.Log("Not exist scenes in builds settings. ");
                }
            }
            else
            {
                ToolsDebug.Log("Not exist scenes in builds settings. ");
            }

#else
            if (EditorBuildSettings.scenes.Length != 0)
                EditorApplication.OpenScene(EditorBuildSettings.scenes[0].path);
            else
            {
                ToolsDebug.Log("Not exist scenes in builds settings. ");  
            }
#endif


        }


        /// <summary>
        /// Handler for open scene in editor event.
        /// </summary>
        /// <param name="scene">Scene</param>
        /// <param name="mode">Scene mode.</param>
        public static void OnSceneOpenedInEditor(Scene scene, OpenSceneMode mode)
        {
            ToolsDebug.Log("OnSceneOpenedInEditor " + sceneHistoryList.Count);
            foreach (string s in sceneHistoryList)
            {
                ToolsDebug.Log(s);
            }
            //Just in editor, non runtime
            if (!Application.isPlaying && mode == OpenSceneMode.Single)
            {

                AddedSceneInHistory(new SceneRef(scene.name));
            }
        }
        public static void AddedSceneInHistory(SceneRef scene)
        {
            if (!userLoadedScene)
            {
                if (sceneHistoryList.Count >= maxLength)
                {
                    sceneHistoryList.RemoveAt(0);
                    sceneHistoryList.Add(new SceneRef(SceneManager.GetActiveScene().path));
                }
                else
                {
                    sceneHistoryList.Add(new SceneRef(SceneManager.GetActiveScene().path));
                }
                Debug.Log("Added");
                string pathTmp = SceneManager.GetActiveScene().path;

                EditorSceneManager.SetActiveScene(EditorSceneManager.GetSceneByName(scene.Name));
                EditorSceneManager.CloseScene(EditorSceneManager.GetSceneByPath(pathTmp), true);
                cursor = sceneHistoryList.Count - 1;
            }
            else
            {
                /*if (sceneHistoryList.Count >= maxLength)
                {
                    sceneHistoryList.RemoveAt(0);
                    sceneHistoryList.Add(scene);
                }
                else
                {
                    sceneHistoryList.Add(scene);
                }
                cursor = sceneHistoryList.Count - 1;*/
            }
            userLoadedScene = false;
        }
#if !UNITY_5_0_OR_NEWER && !UNITY_2017_1_OR_NEWER
        public static void CheckSceneUpdated()
        {
            if (cursor >= 0 && cursor < sceneHistoryList.Count)
            {
                if (sceneHistoryList[cursor] != EditorApplication.currentScene)
                {
                    lastActiveScene = sceneHistoryList[cursor];
                    AddedSceneInHistory(new SceneRef(EditorApplication.currentScene));
                }
            }
        }
#endif
        #endregion Global Methods

        #region Global Constructors
        static SceneHistoryMenu()
        {
#if UNITY_5_0_OR_NEWER || UNITY_2017_1_OR_NEWER
            ToolsDebug.Log("Init history scene");
            EditorSceneManager.sceneOpened += OnSceneOpenedInEditor;
            sceneHistoryList.Add(new SceneRef(SceneManager.GetActiveScene().path));
#else
            EditorApplication.hierarchyWindowChanged += CheckSceneUpdated;
            sceneHistoryList.Add(new SceneRef(EditorApplication.currentScene));
#endif
            cursor = 0;
        }
        #endregion Global Constructors
    }
}