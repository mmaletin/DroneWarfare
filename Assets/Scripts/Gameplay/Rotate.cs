
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 axis;
    public float speed = 100;

    private void Update()
    {
        transform.Rotate(axis, speed * Time.deltaTime, Space.Self);
    }
}