using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Replicator : MonoBehaviour
{
    public GameObject original;
    public GameObject prefab;
    public Vector3 positiveLimit;
    public Vector3 negativeLimit;
    public Space space = Space.World;
    public float distance;

    [Space]

    public float randomDistance = 0;
    public bool randomizeRotation = false;

    [Space]

    public bool replicateOnStart = false;
    public bool removeCloneFromName = false;

    void Start()
    {
        if (replicateOnStart) Replicate();
    }

    public void Replicate(bool withPrefabConnection = false)
    {
        for (int x = -(int)(Mathf.Abs(negativeLimit.x)); x < positiveLimit.x + 1; x++)
        {
            for (int y = -(int)(Mathf.Abs(negativeLimit.y)); y < positiveLimit.y + 1; y++)
            {
                for (int z = -(int)(Mathf.Abs(negativeLimit.z)); z < positiveLimit.z + 1; z++)
                {
                    if (x != 0 || y != 0 || z != 0)
                    {
                        Vector3 replicaPos = original.transform.position + new Vector3(x, y, z) * (distance + Random.Range(-randomDistance / 2, randomDistance / 2));

                        if (space == Space.Self)
                        {
                            replicaPos = original.transform.position + (original.transform.forward * x + original.transform.up * y + original.transform.right * z) * (distance + Random.Range(- randomDistance / 2, randomDistance / 2));
                        }

#if UNITY_EDITOR
                        GameObject replica;

                        if (withPrefabConnection)
                        {
                            replica = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                            replica.transform.SetPositionAndRotation(replicaPos, original.transform.rotation);
                        }
                        else
                        {
                            replica = Instantiate(original, replicaPos, original.transform.rotation);
                        }
#else

                        GameObject replica = Instantiate(original, replicaPos, original.transform.rotation);
#endif

                        replica.transform.parent = original.transform.parent;
                        if (randomizeRotation) replica.transform.rotation = Random.rotationUniform;
                        if (removeCloneFromName) replica.name = replica.name.Replace("(Clone)", "");

#if UNITY_EDITOR
                        Undo.RegisterCreatedObjectUndo(replica, "Replication");
#endif
                    }
                }
            }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Replicator))]
class ReplicatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Replicator replicator = (Replicator)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Replicate"))
        {
            replicator.Replicate();
        }

        if (replicator.prefab != null && GUILayout.Button("Replicate prefabs"))
        {
            replicator.Replicate(true);
        }
    }
}
#endif