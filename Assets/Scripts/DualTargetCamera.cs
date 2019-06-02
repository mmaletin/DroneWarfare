
using UnityEngine;

public class DualTargetCamera : MonoBehaviour
{
    public Transform targetA;
    public Transform targetB;

    public float distanceMP = 2;

    public float minDistance = 10;

    private void LateUpdate()
    {
        if (targetA == null && targetB == null) return;

        if (targetA == null)
        {
            transform.position = Vector3.Lerp(transform.position, targetB.position - transform.forward * minDistance, Time.deltaTime);
            return;
        }

        if (targetB == null)
        {
            transform.position = Vector3.Lerp(transform.position, targetA.position - transform.forward * minDistance, Time.deltaTime);
            return;
        }

        var avg = (targetA.position + targetB.position) * .5f;
        var targetsDistance = (targetA.position - targetB.position).magnitude;

        var distance = Mathf.Max(minDistance, targetsDistance * distanceMP);

        transform.position = avg - transform.forward * distance;
    }
}