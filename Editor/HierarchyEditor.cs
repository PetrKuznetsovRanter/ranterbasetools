using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using RanterTools.Base;

namespace RanterTools.Editor.Base
{

    /// <summary>
    /// Hierarchy editor for custom draw for some types.
    /// </summary>
    [InitializeOnLoad]
    public class HierarchyEditor : MonoBehaviour
    {
        static HierarchyEditor()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
        }

        static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            GameObject objectTmp = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            CheckTypes(objectTmp, selectionRect);
        }

        static void CheckTypes(GameObject obj, Rect selectionRect)
        {
            if (obj == null) return;
            string extras = "";
            List<Component> components = new List<Component>(obj.GetComponents<Component>());
            foreach (var c in components)
            {
                if (c == null) continue;
                System.Type t = c.GetType();
                if (t.BaseType.GetField("instance", BindingFlags.Static | BindingFlags.NonPublic) != null)
                {
                    extras += "s";
                }
                if (c.GetType() == typeof(DontDestroyOnLoad))
                {
                    extras += "d";
                }
            }
            GUIStyle style = new GUIStyle();
            GUIContent content = new GUIContent();
            content.text = extras;
            Rect s = new Rect(selectionRect);
            float multiplier = 1;
            if (obj.transform.childCount != 0) multiplier = 2.5f;
            s.x = s.x - style.CalcSize(content).x - 5 * multiplier;
            GUI.Label(s, extras, style);
        }
    }
}