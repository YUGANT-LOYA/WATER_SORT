using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace YugantLibrary.MyCustomAttributes
{
    [CustomPropertyDrawer(typeof(CustomReadOnlyAttribute))]
    public class ReadOnlyPropertyDrawerScript : UnityEditor.PropertyDrawer
    {


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label);
            GUI.enabled = true;
        }
    }
}
