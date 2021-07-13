using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RanterTools.Base
{

    [Serializable]
    public class BuildSettings
    {
        #region Parameters
        public string buildVersion = $"0";
        public string buildNumberAndroid = $"0";
        public string buildNumberIOS = $"0";
        public string buildNumberWindows = $"0";
        public string buildNumberMacOS = $"0";
        public string buildNumberLinux = $"0";
        #endregion Parameters
        #region State
        public string BuildVersion { get { return buildVersion; } }
        public string BuildNumberAndroid { get { return buildNumberAndroid; } }
        public string BuildNumberIOS { get { return buildNumberIOS; } }
        public string BuildNumberWindows { get { return buildNumberWindows; } }
        public string BuildNumberMacOS { get { return BuildNumberMacOS; } }
        public string BuildNumberLinux { get { return BuildNumberLinux; } }
        #endregion State
    }


    public static class BaseSettings
    {
        #region Parameters
        static BuildSettings buildSettings;
        [SerializeField]
        public static string buildSettingsFileName;
        #endregion Parameters
        #region State
        public static BuildSettings BuildSettings
        {
            get
            {
                LoadOrCreateBuildSettings();
                return buildSettings;
            }
            set
            {
                buildSettings = value;
                SaveOrCreateNew();
            }
        }
        #endregion State
        #region Methods
        public static void LoadOrCreateBuildSettings()
        {
            if (buildSettings == null)
            {
                var file = Resources.Load<TextAsset>(buildSettingsFileName);
                if (file == null)
                {
#if !UNITY_EDITOR
                    //Fill default data in runtime.
                    buildSettings = new BuildSettings();
                    buildSettings.buildVersion = Application.version;
                    buildSettings.buildNumberAndroid = buildSettings.buildNumberIOS =
                    buildSettings.buildNumberLinux = buildSettings.buildNumberMacOS = buildSettings.buildNumberWindows = "0";
#else
                    if (!string.IsNullOrEmpty(buildSettingsFileName))
                    {
                        //Fill data from global build settings and save to resources
                        buildSettings = new BuildSettings();
                        buildSettings.buildVersion = PlayerSettings.bundleVersion;
                        buildSettings.buildNumberAndroid = $"{PlayerSettings.Android.bundleVersionCode}";
                        buildSettings.buildNumberIOS = $"{PlayerSettings.iOS.buildNumber}";
                        buildSettings.buildNumberLinux = buildSettings.buildNumberMacOS = buildSettings.buildNumberWindows = $"{PlayerSettings.macOS.buildNumber}";
                        if (!Directory.Exists(Path.GetDirectoryName(buildSettingsFileName)))
                            Directory.CreateDirectory(Path.GetDirectoryName(buildSettingsFileName));
                        AssetDatabase.CreateAsset(new TextAsset(JsonUtility.ToJson(buildSettings)), $"{buildSettingsFileName}");
                        AssetDatabase.SaveAssets();
                    }
#endif
                }
                else buildSettings = JsonUtility.FromJson<BuildSettings>(file.text);
            }
        }
        public static void SaveOrCreateNew()
        {
            LoadOrCreateBuildSettings();
#if UNITY_EDITOR
            //Fill data from global build settings and save to resources
            if (!Directory.Exists(Path.GetDirectoryName(buildSettingsFileName)))
                Directory.CreateDirectory(Path.GetDirectoryName(buildSettingsFileName));
            AssetDatabase.DeleteAsset(BaseSettings.buildSettingsFileName);
            AssetDatabase.CreateAsset(new TextAsset(JsonUtility.ToJson(buildSettings)), $"{ BaseSettings.buildSettingsFileName}");
            AssetDatabase.SaveAssets();
#endif
        }

#if UNITY_EDITOR
        public static void UpdateBuildNumber(int increment = 1)
        {
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.Android: buildSettings.buildNumberAndroid += increment; break;
                case BuildTarget.iOS: buildSettings.buildNumberIOS += increment; break;
                case BuildTarget.StandaloneLinux64: buildSettings.buildNumberLinux += increment; break;
                case BuildTarget.StandaloneOSX: buildSettings.buildNumberMacOS += increment; break;
                case BuildTarget.StandaloneWindows64: buildSettings.buildNumberWindows += increment; break;
                default: break;
            }
            SaveOrCreateNew();
        }
#endif
        #endregion Methods
    }

}
