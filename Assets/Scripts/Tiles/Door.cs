
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    public Transform doorMesh;

    public Vector3 posOpened, posClosed;

    public float lerpPerSecond = 1;

    public bool open = false;

    public UnityEvent onOpened;
    public UnityEvent onClosed;

    private void Update()
    {
        doorMesh.localPosition = Vector3.Lerp(doorMesh.localPosition, open ? posOpened : posClosed, lerpPerSecond * Time.deltaTime);
    }

    public void SetOpen(bool value)
    {
        open = value;

        if (open) onOpened.Invoke(); else onClosed.Invoke();
    }
}