using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
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

    // Returns all List<T> fields on the managed reference object
    private List<FieldInfo> GetListFields(object obj)
    {
        if (obj == null) return new();
        return obj.GetType()
            .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(f => f.FieldType.IsGenericType &&
                        f.FieldType.GetGenericTypeDefinition() == typeof(List<>))
            .ToList();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var dataProp = property.FindPropertyRelative("data");
        float height = EditorGUIUtility.singleLineHeight * 2 + 6;

        if (dataProp.managedReferenceValue != null)
        {
            height += EditorGUI.GetPropertyHeight(dataProp, true);

            // Add height for each list field's add button + element labels
            foreach (var listField in GetListFields(dataProp.managedReferenceValue))
            {
                var list = listField.GetValue(dataProp.managedReferenceValue) as System.Collections.IList;
                int count = list?.Count ?? 0;
                // Add button row + each element row
                height += EditorGUIUtility.singleLineHeight * (1 + count) + 4;
            }
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

        // Type dropdown
        var types = GetDerivedTypes();
        var typeNames = types.Select(t => t.Name).ToArray();
        int currentIndex = -1;

        if (dataProp.managedReferenceValue != null)
        {
            var currentType = dataProp.managedReferenceValue.GetType();
            currentIndex = types.IndexOf(currentType);
        }

        int selectedIndex = EditorGUI.Popup(
            new Rect(position.x, y, position.width, lineHeight),
            "Data Type",
            currentIndex,
            typeNames
        );

        if (selectedIndex != currentIndex && selectedIndex >= 0)
            dataProp.managedReferenceValue = Activator.CreateInstance(types[selectedIndex]);

        y += lineHeight + 2;

        // Draw data fields
        if (dataProp.managedReferenceValue != null)
        {
            float dataHeight = EditorGUI.GetPropertyHeight(dataProp, true);
            EditorGUI.PropertyField(
                new Rect(position.x, y, position.width, dataHeight),
                dataProp,
                true
            );
            y += dataHeight;

            // Draw list controls for each List<T> field found on the data object
            DrawListFields(dataProp, position, ref y, lineHeight);
        }

        EditorGUI.EndProperty();
    }

    private void DrawListFields(SerializedProperty dataProp, Rect position, ref float y, float lineHeight)
    {
        var target = dataProp.managedReferenceValue;
        var listFields = GetListFields(target);

        foreach (var listField in listFields)
        {
            var list = listField.GetValue(target) as System.Collections.IList;
            Type elementType = listField.FieldType.GetGenericArguments()[0];

            // Initialize list if null
            if (list == null)
            {
                list = (System.Collections.IList)Activator.CreateInstance(
                    typeof(List<>).MakeGenericType(elementType));
                listField.SetValue(target, list);
            }

            // Label for the list
            EditorGUI.LabelField(
                new Rect(position.x, y, position.width, lineHeight),
                listField.Name,
                EditorStyles.boldLabel
            );
            y += lineHeight + 2;

            // Draw each element as an object field
            for (int i = 0; i < list.Count; i++)
            {
                float fieldWidth = position.width - 22;

                list[i] = EditorGUI.ObjectField(
                    new Rect(position.x, y, fieldWidth, lineHeight),
                    list[i] as UnityEngine.Object,
                    elementType,
                    false  // set true if you want scene objects too
                );

                // Remove button
                if (GUI.Button(new Rect(position.x + fieldWidth + 2, y, 20, lineHeight), "-"))
                {
                    list.RemoveAt(i);
                    // Mark dirty so changes are saved
                    EditorUtility.SetDirty(dataProp.serializedObject.targetObject);
                    break;
                }

                y += lineHeight + 2;
            }

            // Add button
            if (GUI.Button(new Rect(position.x, y, position.width, lineHeight), $"+ Add {elementType.Name}"))
            {
                list.Add(null);
                EditorUtility.SetDirty(dataProp.serializedObject.targetObject);
            }
            y += lineHeight + 4;
        }
    }
}