
using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnsureInterfaceImplementationAttribute : PropertyAttribute {

    public Type type { get; private set; }
    public string labelText { get; private set; }

	public EnsureInterfaceImplementationAttribute(Type type, string labelText = "")
    {
        this.type = type;
        this.labelText = labelText;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(EnsureInterfaceImplementationAttribute))]
public class EnsureInterfaceImplementationAttributeDrawer : PropertyDrawer
{
    override public void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var interfaceAttribute = attribute as EnsureInterfaceImplementationAttribute;

        if (!string.IsNullOrEmpty(interfaceAttribute.labelText)) label.text = ObjectNames.NicifyVariableName(interfaceAttribute.labelText);


        EditorGUI.BeginDisabledGroup(Application.isPlaying);

        EditorGUI.PropertyField(position, property, label);

        EditorGUI.EndDisabledGroup();


        if ((property.objectReferenceValue as GameObject)?.GetComponent(interfaceAttribute.type) == null)
        {
            property.objectReferenceValue = null;
        }
    }
}
#endif