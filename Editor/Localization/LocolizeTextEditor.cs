using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.IO;
using RanterTools.Localization;
using RanterTools.Base;
namespace RanterTools.Editor.Localization
{
    /// <summary>
    /// Localized text editor window.
    /// </summary>
    public class LocalizedTextEditor : EditorWindow
    {
        #region Global State
        static LocalizedTextEditor localizedTextEditor;
        #endregion Global State
        #region State
        /// <summary>
        /// List of key value pair for localized dictionary.
        /// </summary>
        public LocalizationData localizationData;
        Vector2 scrollPosition = Vector2.zero;
        #endregion State
        /// <summary>
        /// Init window method.
        /// </summary>
        [MenuItem(GlobalNames.MenuRanterTools + "/" + GlobalNames.MenuLocalization + "/" + GlobalNames.MenuLocalizationEditor)]
        static void Init()
        {
            if (localizedTextEditor == null)
            {
                localizedTextEditor = (LocalizedTextEditor)EditorWindow.GetWindow(typeof(LocalizedTextEditor));
            }
            localizedTextEditor.Show();
        }

        /// <summary>
        /// Rapint window.
        /// </summary>        
        private void OnGUI()
        {
            if (localizationData != null)
            {
                SerializedObject serializedObject = new SerializedObject(this);
                SerializedProperty serializedProperty = serializedObject.FindProperty("localizationData");

                using (var s = new EditorGUILayout.ScrollViewScope(scrollPosition))
                {
                    scrollPosition = s.scrollPosition;
                    serializedProperty = serializedProperty.FindPropertyRelative("items");
                    SerializedProperty element;
                    for (int i = 0; i < serializedProperty.arraySize; i++)
                    {
                        element = serializedProperty.GetArrayElementAtIndex(i);
                        EditorGUILayout.PropertyField(element, true);
                    }

                }
                serializedObject.ApplyModifiedProperties();

                if (GUILayout.Button("Save data"))
                {
                    SaveGameData();
                }
            }
            if (GUILayout.Button("Load data"))
            {
                LoadGameData();
            }
            if (GUILayout.Button("Create new data"))
            {
                CreateNewData();
            }
        }

        /// <summary>
        /// Show file dialog window for load localized data from file.
        /// </summary>
        private void LoadGameData()
        {
            string filePath = EditorUtility.OpenFilePanel("Select localization data file", Application.streamingAssetsPath, "json");

            if (!string.IsNullOrEmpty(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);

                localizationData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
            }
        }
        /// <summary>
        /// Show file dialog window for save localized data from file.
        /// </summary>
        private void SaveGameData()
        {
            string filePath = EditorUtility.SaveFilePanel("Save localization data file", Application.streamingAssetsPath, "", "json");

            if (!string.IsNullOrEmpty(filePath))
            {
                string dataAsJson = JsonUtility.ToJson(localizationData);
                File.WriteAllText(filePath, dataAsJson);
            }
        }

        /// <summary>
        /// Create new localized data.
        /// </summary>
        private void CreateNewData()
        {
            localizationData = new LocalizationData();
        }

    }

    [CustomPropertyDrawer(typeof(LocalizationItem))]
    public class CustomPropertyDrawerLocalizationItem : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            var keyRect = new Rect(position.x, position.y, position.width * 0.3f, position.height);
            var valueRect = new Rect(position.x + position.width * 0.3f, position.y, position.width * 0.7f, position.height);

            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(keyRect, property.FindPropertyRelative("key"), GUIContent.none);
            EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("value"), GUIContent.none);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }

}