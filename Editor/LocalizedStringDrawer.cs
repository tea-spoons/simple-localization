
namespace TeaSpoons.SimpleLocalization.Editor
{
    using UnityEngine;
    using UnityEditor;
    using System.Reflection;

    [CustomPropertyDrawer(typeof(LocalizedString))]
    public class LocalizedStringDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label.image = EditorGUIUtility.IconContent("d_FilterByLabel").image;

            var keyProperty = property.FindPropertyRelative("key");
            EditorGUI.PropertyField(position, keyProperty, label);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (fieldInfo.GetCustomAttribute<LocalizedTextAreaAttribute>() != null)
            {
                return 80;
            }

            return base.GetPropertyHeight(property, label);
        }
    }
}
