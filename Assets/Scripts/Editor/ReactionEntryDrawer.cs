// ReactionEntryDrawer.cs
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ReactionEntry))]
public class ReactionEntryDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var triggerProp = property.FindPropertyRelative("trigger");
        var effectProp = property.FindPropertyRelative("effect");

        return EditorGUIUtility.singleLineHeight + 2          // "trigger" field
             + EditorGUI.GetPropertyHeight(effectProp, true)  // full EffectEntry (uses your existing drawer)
             + 6;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        float lineH = EditorGUIUtility.singleLineHeight;
        float y = position.y;

        var triggerProp = property.FindPropertyRelative("trigger");
        var effectProp = property.FindPropertyRelative("effect");

        // Trigger object field
        EditorGUI.PropertyField(
            new Rect(position.x, y, position.width, lineH),
            triggerProp, new GUIContent("Trigger")
        );
        y += lineH + 2;

        // Effect entry — your existing EffectEntryDrawer handles this automatically
        float effectHeight = EditorGUI.GetPropertyHeight(effectProp, true);
        EditorGUI.PropertyField(
            new Rect(position.x, y, position.width, effectHeight),
            effectProp, new GUIContent("Effect"), true
        );

        EditorGUI.EndProperty();
    }
}