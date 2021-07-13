using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using RanterTools.Base;

namespace RanterTools.Localization
{
    /// <summary>
    /// Localization manager single in runtime.
    /// </summary>
    [RequireComponent(typeof(DontDestroyOnLoad))]
    public class LocalizationManager : SingletonBehaviour<LocalizationManager>
    {
        #region Parameters
        [SerializeField]
        string missingTextString = "Localized text not found";
        #endregion Parameters

        #region State
        Dictionary<string, string> localizedText = new Dictionary<string, string>();
        bool isReady = false;
        /// <summary>
        /// Are all texts uploaded
        /// </summary>
        /// <value>Is ready</value>
        public bool IsReady
        {
            get { return isReady; }
        }
        #endregion State

        #region Global Methods
        /// <summary>
        /// Get localized value
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public static string GetLocalizedValue(string key)
        {
            string result = Instance.missingTextString;
            if (Instance.localizedText.ContainsKey(key))
            {
                result = Instance.localizedText[key];
            }
            return result;
        }
        #endregion Global Methods

        #region Methods

        void LoadLocalizedText(string fileName)
        {
            string filePath;
            string dataAsJson;
            localizedText = new Dictionary<string, string>();

            LocalizationData loadedData;
            if (Application.platform == RuntimePlatform.Android)
            {
                filePath = Path.Combine(Application.streamingAssetsPath, fileName);
                UnityWebRequest reader = new UnityWebRequest(filePath);
                while (!reader.isDone) { }
                dataAsJson = reader.downloadHandler.text;

                loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
                for (int i = 0; i < loadedData.items.Length; i++)
                {
                    localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
                }

                Debug.Log("Data loaded, dictionary contains: " + localizedText.Count + " entries");
            }
            else
            {
                filePath = Path.Combine(Application.streamingAssetsPath, fileName);
                if (File.Exists(filePath))
                {

                    dataAsJson = File.ReadAllText(filePath);
                    loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

                    for (int i = 0; i < loadedData.items.Length; i++)
                    {
                        localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
                    }

                    Debug.Log("Data loaded, dictionary contains: " + localizedText.Count + " entries");
                }
                else
                {
                    Debug.LogError("Cannot find file!");
                }
            }
            isReady = true;
        }
        #endregion Methods


        #region Unity
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.Russian:
                    {
                        LoadLocalizedText("Localization/ru/strings.json");
                        break;
                    }
                case SystemLanguage.English:
                    {
                        LoadLocalizedText("Localization/en/strings.json");
                        break;
                    }
                default:
                    {
                        LoadLocalizedText("Localization/ru/strings.json");
                        break;
                    }
            }
        }
        #endregion Unity
    }

}