using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
namespace RanterTools.Base
{
    public class BuildSettingsWindow : EditorWindow
    {
        static BuildSettingsWindow Instance = null;
        
        [MenuItem(GlobalNames.MenuRanterTools + "/" + GlobalNames.BuildSettings + "/" + GlobalNames.BuildSettingsMenu)]
        public static void BuildSettingMenu()
        {
            var windows = Resources.FindObjectsOfTypeAll<BuildSettingsWindow>();
            if (windows != null && windows.Length != 0)
                foreach (var window in windows)
                {
                    window.Close();
                    DestroyImmediate(window);
                }
            Instance = GetWindow<BuildSettingsWindow>();
            Instance.Show();
            Instance.titleContent = new GUIContent(GlobalNames.BuildSettingsMenu);
            if (string.IsNullOrEmpty(BaseSettings.buildSettingsFileName))
            {
                BaseSettings.buildSettingsFileName = "Assets/DefaultBuildSettings.asset";
            }
        }
        
        /// <summary>
        /// OnGUI is called for rendering and handling GUI events.
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        void OnGUI()
        {
            using (var h = new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Build version:", GUILayout.Width(150));
                BaseSettings.BuildSettings.buildVersion = EditorGUILayout.TextField(BaseSettings.BuildSettings.buildVersion);
            }
            using (var h = new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Android build number:", GUILayout.Width(150));
                BaseSettings.BuildSettings.buildNumberAndroid = EditorGUILayout.TextField(BaseSettings.BuildSettings.buildNumberAndroid);
            }
            using (var h = new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("iOS build number:", GUILayout.Width(150));
                BaseSettings.BuildSettings.buildNumberIOS = EditorGUILayout.TextField(BaseSettings.BuildSettings.buildNumberIOS);
            }
            using (var h = new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Windows build number:", GUILayout.Width(150));
                BaseSettings.BuildSettings.buildNumberWindows = EditorGUILayout.TextField(BaseSettings.BuildSettings.buildNumberWindows);
            }
            using (var h = new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("MacOS build number:", GUILayout.Width(150));
                BaseSettings.BuildSettings.buildNumberMacOS = EditorGUILayout.TextField(BaseSettings.BuildSettings.buildNumberMacOS);
            }
            using (var h = new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Linux build number:", GUILayout.Width(150));
                BaseSettings.BuildSettings.buildNumberLinux = EditorGUILayout.TextField(BaseSettings.BuildSettings.buildNumberLinux);
            }
            using (var h = new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Build settings file path:", GUILayout.Width(150));
                BaseSettings.buildSettingsFileName = EditorGUILayout.TextField(BaseSettings.buildSettingsFileName);
                if (GUILayout.Button("Choose file", GUILayout.Width(150)))
                {

                    if (!Directory.Exists(Path.Combine(Application.dataPath, "Resources")))
                        Directory.CreateDirectory(Path.Combine(Application.dataPath, "Resources"));
                    BaseSettings.buildSettingsFileName = EditorUtility.SaveFilePanelInProject("Choose where save build settings.",
                    "DefaultSettings", "asset", "Message", Path.Combine(Application.dataPath, "Resources"));//Path.Combine(EditorApplication.applicationContentsPath, "Asset/Resources"));
                    BaseSettings.buildSettingsFileName = BaseSettings.buildSettingsFileName.Replace(Application.dataPath, "Assets/");
                }
            }
            using (var h = new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Save"))
                {
                    BaseSettings.SaveOrCreateNew();
                }
                if (GUILayout.Button("Load"))
                {
                    BaseSettings.LoadOrCreateBuildSettings();
                }
                if (GUILayout.Button("Delete"))
                {
                    AssetDatabase.DeleteAsset(BaseSettings.buildSettingsFileName);
                }
            }

        }
    }

    class MyCustomBuildProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }
        public void OnPreprocessBuild(BuildReport report)
        {
            Debug.Log($"{report.summary.options} {report.name} {report.summary.options} ");
        }
        public void OnPostprocessBuild(BuildReport report)
        {
            Debug.Log("MyCustomBuildProcessor.OnPostprocessBuild for target " + report.summary.platform + " at path " + report.summary.outputPath);
        }
    }

}
