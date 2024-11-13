using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entity;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EntityStatBase), true)]
public class EntityStatCustomDrawer : PropertyDrawer
{
    List<System.Type> m_DerivedTypes = new List<System.Type>();
    
    public override void OnGUI(Rect drawerRect, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(drawerRect, label, property);
        
        string type = property.managedReferenceValue?.GetType().Name ?? "null";
        drawerRect = EditorGUI.PrefixLabel(drawerRect, GUIUtility.GetControlID(FocusType.Passive), new GUIContent(type));
        var typeNames = GetDerivedTypes().Select(type => type.Name).ToList();
        int selectedIndex = typeNames.IndexOf(type);

        GUIContent[] typeOptions = typeNames.Select(name => new GUIContent(name)).ToArray();
        selectedIndex = EditorGUI.Popup(
            new Rect(drawerRect.x, drawerRect.y, drawerRect.width, EditorGUIUtility.singleLineHeight),selectedIndex, typeOptions);
        
        if (selectedIndex >= 0 && typeNames[selectedIndex] != type)
        {
            var selectedType = GetDerivedTypes()[selectedIndex];
            property.managedReferenceValue = Activator.CreateInstance(selectedType);
        }

        if (property.managedReferenceValue != null)
        {
            EditorGUI.indentLevel++;
            foreach (SerializedProperty prop in property)
            {
                drawerRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(new Rect(0, drawerRect.y, drawerRect.width, EditorGUI.GetPropertyHeight(prop, true)), prop, true);
            }
            EditorGUI.indentLevel--;
        }
        
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUIUtility.singleLineHeight;
        
        if (property.managedReferenceValue != null)
        {
            foreach (SerializedProperty prop in property)
            {
                height += EditorGUI.GetPropertyHeight(prop, true) + EditorGUIUtility.standardVerticalSpacing;
            }
        }

        return height;
    }

    private List<System.Type> GetDerivedTypes()
    {
        if (m_DerivedTypes.Count == 0)
        {
            //Cache type values since this logic is quite performance intensive
            m_DerivedTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(EntityStatBase)) && !type.IsAbstract)
                .ToList();
        }

        return m_DerivedTypes;
    }
}
