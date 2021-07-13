using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace RanterTools.Scenes
{
    /// <summary>
    /// SceneRef type property drawer.
    /// </summary>
    [CustomPropertyDrawer(typeof(SceneRef))]
    public class SceneRefDrawer : PropertyDrawer
    {
        /// <summary>
        /// Rapint property.
        /// </summary> 
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, GUIContent.none, property);
            SerializedProperty sceneAsset = property.FindPropertyRelative("sceneAsset");
            SerializedProperty sceneName = property.FindPropertyRelative("name");
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            if (sceneAsset != null)
            {
                sceneAsset.objectReferenceValue = EditorGUI.ObjectField(position, sceneAsset.objectReferenceValue, typeof(SceneAsset), false);
                if (sceneAsset.objectReferenceValue != null)
                {
                    sceneName.stringValue = (sceneAsset.objectReferenceValue as SceneAsset).name;
                }
            }
            EditorGUI.EndProperty();
        }
    }
}