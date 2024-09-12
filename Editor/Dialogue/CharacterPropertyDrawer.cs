using UnityEditor;
using IvoriesStudios.LevelScripting.Dialogue;
using UnityEngine;

namespace IvoriesStudios.LevelScripting.Editor
{
    [CustomPropertyDrawer(typeof(Character))]
    public class CharacterPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            Rect nameRect = new Rect(position.x, position.y, position.width - 70, position.height / 2);
            Rect spriteRect = new Rect(nameRect.position.x + nameRect.width + 10, position.y, 60, position.height);

            EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("<Name>k__BackingField"), GUIContent.none);
            SerializedProperty portraitProperty = property.FindPropertyRelative("<Portrait>k__BackingField");
            portraitProperty.objectReferenceValue = EditorGUI.ObjectField(spriteRect, portraitProperty.objectReferenceValue, typeof(Sprite), false);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            //set the height of the drawer by the field size and padding
            return 60;
        }
    }
}
