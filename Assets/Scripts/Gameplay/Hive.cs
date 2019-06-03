
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Hive : MonoBehaviour
{
    public List<PlayerRobot> playerRobots { get; private set; }
    public PlayerRobot main;

    public HiveLink hiveLinkPrefab;
    private Dictionary<PlayerRobot, HiveLink> hiveLinks = new Dictionary<PlayerRobot, HiveLink>();

    public LayerMask layerMask;

    private void Start()
    {
        playerRobots = ComponentsFinder.Find<PlayerRobot>();

        main = (from pr in playerRobots where pr.main select pr).Single();

        foreach (var pr in playerRobots)
        {
            var link = Instantiate(hiveLinkPrefab, transform);
            link.from = main.raycastPoint;
            link.to = pr.raycastPoint;

            hiveLinks.Add(pr, link);
        }
    }

    private void Update()
    {
        foreach (var pr in playerRobots)
        {
            if (pr == main) continue;

            if (pr == null)
            {
                hiveLinks[pr].active = false;
                continue;
            }

            var ray = new Ray(pr.raycastPoint.position, main.raycastPoint.position - pr.raycastPoint.position);

            Debug.DrawRay(ray.origin, ray.direction, Color.red, Time.deltaTime);

            if (Physics.Raycast(ray, out var hit, 100, layerMask))
            {
                pr.activated = hit.transform.IsChildOf(main.transform);
            }
            else
            {
                pr.activated = false;
            }

            hiveLinks[pr].active = pr.activated;
        }
    }
}