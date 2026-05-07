using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(EffectEntry))]
public class EffectEntryDrawer : PropertyDrawer
{
    private List<Type> GetDerivedTypes()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsSubclassOf(typeof(EffectData)) && !t.IsAbstract)
            .ToList();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var dataProp = property.FindPropertyRelative("data");

        float height = EditorGUIUtility.singleLineHeight * 2 + 6;

        if (dataProp.managedReferenceValue != null)
        {
            height += EditorGUI.GetPropertyHeight(dataProp, true);
        }

        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var effectProp = property.FindPropertyRelative("effect");
        var dataProp = property.FindPropertyRelative("data");

        float lineHeight = EditorGUIUtility.singleLineHeight;
        float y = position.y;

        // Draw Effect field
        EditorGUI.PropertyField(
            new Rect(position.x, y, position.width, lineHeight),
            effectProp
        );

        y += lineHeight + 2;

        // Get types
        var types = GetDerivedTypes();
        var typeNames = types.Select(t => t.Name).ToArray();

        int currentIndex = -1;

        if (dataProp.managedReferenceValue != null)
        {
            var currentType = dataProp.managedReferenceValue.GetType();
            currentIndex = types.IndexOf(currentType);
        }

        // Dropdown
        int selectedIndex = EditorGUI.Popup(
            new Rect(position.x, y, position.width, lineHeight),
            "Data Type",
            currentIndex,
            typeNames
        );

        // If changed → create new instance
        if (selectedIndex != currentIndex && selectedIndex >= 0)
        {
            dataProp.managedReferenceValue = Activator.CreateInstance(types[selectedIndex]);
        }

        y += lineHeight + 2;

        // Draw data fields
        if (dataProp.managedReferenceValue != null)
        {
            EditorGUI.PropertyField(
                new Rect(position.x, y, position.width, EditorGUI.GetPropertyHeight(dataProp, true)),
                dataProp,
                true
            );
        }

        EditorGUI.EndProperty();
    }
}
