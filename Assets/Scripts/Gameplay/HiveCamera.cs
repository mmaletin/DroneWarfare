
using UnityEngine;

public class HiveCamera : MonoBehaviour
{
    public float distanceMP = 2;

    public float minDistance = 15;

    public Hive hive;

    public float speed = 5;

    private float distance;

    private void Start()
    {
        distance = minDistance;
    }

    private void LateUpdate()
    {
        if (hive.main == null) return;

        float maxBotDistance = 0;

        foreach (var pr in hive.playerRobots)
        {
            if (pr == null || !pr.activated || pr.main) continue;

            var botDistance = (hive.main.raycastPoint.position - pr.raycastPoint.position).magnitude;

            if (botDistance > maxBotDistance)
                maxBotDistance = botDistance;
        }

        var targetDistance = Mathf.Max(minDistance, maxBotDistance * distanceMP);

        distance = Mathf.Lerp(distance, targetDistance, speed * Time.deltaTime);

        transform.position = hive.main.raycastPoint.position - transform.forward * distance;
    }
}