using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowIfAttribute showIf = attribute as ShowIfAttribute;

        SerializedProperty boolProp =
            property.serializedObject.FindProperty(showIf.boolName);

        if (boolProp != null && boolProp.boolValue)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ShowIfAttribute showIf = attribute as ShowIfAttribute;

        SerializedProperty boolProp =
            property.serializedObject.FindProperty(showIf.boolName);

        if (boolProp != null && boolProp.boolValue)
        {
            return EditorGUI.GetPropertyHeight(property);
        }

        return 0;
    }
}