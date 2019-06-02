
using UnityEngine;
using UnityEditor;

public class PrefabReplacerWindow : EditorWindow
{
    const string replacementUndo = "Prefabs replacement";

    PrefabReplacementGroup prefabReplacementGroup;

    [MenuItem("MM/Prefab replacer")]
    static void Init()
    {
        var window = GetWindow<PrefabReplacerWindow>();
        window.Show();
    }

    private void OnGUI()
    {
        prefabReplacementGroup = EditorGUILayout.ObjectField("Replacement group", prefabReplacementGroup, typeof(PrefabReplacementGroup), false) as PrefabReplacementGroup;

        GUILayout.Space(10);

        if (!prefabReplacementGroup) return;

        foreach (var prefab in prefabReplacementGroup.prefabs)
        {
            if (prefab && GUILayout.Button(prefab.name))
            {
                ReplaceSelectedWith(prefab);
            }
        }
    }

    private static void ReplaceSelectedWith(GameObject prefab)
    {
        foreach (var selected in Selection.gameObjects)
        {
            var instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

            Undo.RegisterCreatedObjectUndo(instance, replacementUndo);

            instance.transform.SetParent(selected.transform.parent);
            instance.transform.SetSiblingIndex(selected.transform.GetSiblingIndex());

            instance.transform.SetPositionAndRotation(selected.transform.position, selected.transform.rotation);

            Undo.DestroyObjectImmediate(selected);
        }
    }
}