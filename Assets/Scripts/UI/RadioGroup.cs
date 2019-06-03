using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RadioGroup : MonoBehaviour {

    [System.Serializable]
    public class SelectionEvent : UnityEvent<int> { }

    public List<Button> buttons;

    public int firstPressed = 0;

    public SelectionEvent onSelected;

    private Dictionary<Button, UnityAction> listeners = new Dictionary<Button, UnityAction>();

    void Awake ()
    {
        foreach (var button in buttons)
        {
            Add(button);
        }

        if (firstPressed >= 0) Select(firstPressed);
	}

    public void Add(Button button)
    {
        if (buttons.Contains(button))
        {
            if (listeners.ContainsKey(button)) return;
        }
        else
        {
            buttons.Add(button);
        }

        listeners.Add(button, () => OnClick(button));
        button.onClick.AddListener(listeners[button]);
    }

    public void Select(int id)
    {
        if (buttons.Count <= id || id < 0) return;

        Button button = buttons[id];

        if (button.interactable) button.onClick.Invoke();
    }

    void OnClick(Button button)
    {
        foreach (var otherButton in buttons)
        {
            otherButton.interactable = true;
        }
        
        button.interactable = false;

        onSelected.Invoke(buttons.IndexOf(button));
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(RadioGroup))]
[CanEditMultipleObjects]
public class RadioGroupEditor : Editor
{
    SerializedProperty buttons;
    SerializedProperty firstPressed;
    SerializedProperty onSelected;

    void OnEnable()
    {
        buttons = serializedObject.FindProperty("buttons");
        firstPressed = serializedObject.FindProperty("firstPressed");
        onSelected = serializedObject.FindProperty("onSelected");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(firstPressed);
        EditorGUILayout.PropertyField(buttons, true);

        if (!serializedObject.isEditingMultipleObjects)
        {
            if (GUILayout.Button("Add child buttons"))
            {
                Undo.RecordObject(serializedObject.targetObject, "Adding child buttons");

                var buttonsToAdd = (serializedObject.targetObject as Component).GetComponentsInChildren<Button>();

                var existingButtons = (serializedObject.targetObject as RadioGroup).buttons;

                foreach (var button in buttonsToAdd)
                {
                    if (!existingButtons.Contains(button))
                    {
                        buttons.InsertArrayElementAtIndex(buttons.arraySize);
                        var buttonProperty = buttons.GetArrayElementAtIndex(buttons.arraySize - 1);

                        buttonProperty.objectReferenceValue = button;
                    }
                }
            }
        }

        EditorGUILayout.PropertyField(onSelected);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif