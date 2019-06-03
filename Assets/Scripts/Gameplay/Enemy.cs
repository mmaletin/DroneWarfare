
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IHitPoints
{
    public GameObject root;

    public Transform gunsOrigin;
    public Transform[] guns;

    public int maxHP = 10;
    private int hp;

    private NavMeshAgent ai;
    private List<PlayerRobot> playerRobots;

    private PlayerRobot target;
    private NavMeshPath tempPath;

    public Shot shotPrefab;
    public float shotDelayMin = 1.2f;
    public float shotDelayMax = 1.8f;

    public float pathReevaluationMin = 1;
    public float pathReevaluationMax = 2;

    private bool activated = false;

    private void Awake()
    {
        ai = GetComponentInParent<NavMeshAgent>();
        tempPath = new NavMeshPath();

        playerRobots = ComponentsFinder.Find<PlayerRobot>();

        hp = maxHP;
    }

    private void OnEnable()
    {
        StartCoroutine(PathReevaluation());

        StartCoroutine(AutoShooter());
    }

    private IEnumerator AutoShooter()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(shotDelayMin, shotDelayMax));

            // Check for target instead?
            if (activated)
            {
                foreach (var gun in guns)
                {
                    var shot = Instantiate(shotPrefab, gun.position, gun.rotation);
                    shot.owner = root;
                }
            }
        }
    }

    // Separate target reevaluation so enemies don't form a big pile
    // Choose target randomly, closest target having highest weight

    private IEnumerator PathReevaluation()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(pathReevaluationMin, pathReevaluationMax));

            PlayerRobot closest = null;
            float minDistance = float.MaxValue;

            foreach (var player in playerRobots)
            {
                // TODO This code will have to call get components or something once i get respawn mechanic
                if (player == null) continue;
                if (!player.activated && !player.main) continue;

                ai.CalculatePath(player.transform.position, tempPath);

                if (tempPath.status != NavMeshPathStatus.PathComplete) continue;

                var distance = GetPathDistance(tempPath);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = player;
                }
            }

            if (closest != null)
            {
                ai.destination = closest.transform.position;

                activated = true;
            }
        }
    }

    private static float GetPathDistance(NavMeshPath path)
    {
        float distance = 0;

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            distance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        }

        return distance;
    }

    public void Hit()
    {
        // TODO Add explisions
        if (hp-- <= 0) Destroy(root);
    }
}