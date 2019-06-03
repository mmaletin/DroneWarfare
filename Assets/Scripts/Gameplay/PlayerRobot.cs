
using System.Collections;
using UnityEngine;

public class PlayerRobot : MonoBehaviour, IHitPoints
{
    private Rigidbody m_body;
    private Rigidbody body { get { if (m_body == null) m_body = GetComponent<Rigidbody>(); return m_body; } }

    public float speed = 3;

    public Transform raycastPoint;

    public Transform towerPivot;
    public Transform[] guns;

    private Camera cam;

    public Shot shot;
    public float gunCooldown = 0.5f;
    private float lastShotTime = 0;

    public int maxHP = 20;
    private int hp;

    public AudioSource audioSource;

    private ITile currentTile;

    public bool main = false;

    // Might want to make variables for activated and active
    // so enemies can target player bots that were activated at least once
    public bool activated = false;

    public bool overLava { get; set; } = false;

    public GameObject lavaHitEffect;

    private void Start()
    {
        cam = Camera.main;

        hp = maxHP;

        CheckTile();

        StartCoroutine(LavaDamageCheck());
    }

    private void Update()
    {
        if (MainMenu.instance.visible || GameOver.instance.visible) return;

        if (activated || main)
        {
            Move();

            Rotate();

            Shoot();
        }

        CheckTile();
    }

    private void LateUpdate()
    {
        if (activated || main)
        {
            ActionProgressIndicator.ShowFor(transform, (float)hp / maxHP);
        }
        else
        {
            ActionProgressIndicator.HideFor(transform);

        }
    }

    private void Move()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(h, 0, v).normalized;

        if (direction != Vector3.zero)
            body.velocity = direction * speed;
    }

    private void Rotate()
    {
        var surface = new Plane(Vector3.up, raycastPoint.position);
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        surface.Raycast(ray, out var distance);

        var targetPoint = ray.GetPoint(distance);

        towerPivot.rotation = Quaternion.Lerp(
            towerPivot.rotation,
            Quaternion.LookRotation(targetPoint - raycastPoint.position),
            Time.deltaTime * 20);
    }

    private void Shoot()
    {
        if (Input.GetMouseButton(0))
        {
            if (Time.time >= lastShotTime + gunCooldown)
            {
                foreach (var gun in guns)
                {
                    var shotInstance = Instantiate(shot, gun.position, gun.rotation);

                    shotInstance.owner = gameObject;
                }

                lastShotTime = Time.time;

                audioSource.volume = Random.Range(.5f, .7f);
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.Play();
            }
        }
    }

    public void Hit()
    {
        // TODO Add explisions
        if (--hp <= 0)
        {
            ActionProgressIndicator.HideFor(transform);

            if (main)
                GameOver.instance.visible = true;

            Destroy(gameObject);
        }
    }

    private void CheckTile()
    {
        RaycastHit hit;

        Ray ray = new Ray(raycastPoint.position, Vector3.down);

        if (Physics.Raycast(ray, out hit))
        {
            var tile = hit.transform.GetComponentInParent<ITile>();

            if (tile == currentTile) return;

            if (currentTile != null)
            {
                currentTile.Deactivate(this);
            }

            if (tile != null)
            {
                tile.Activate(this);
            }

            currentTile = tile;
        }
    }

    private IEnumerator LavaDamageCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            if (overLava)
            {
                Hit();
                Instantiate(lavaHitEffect, transform.position, Quaternion.identity);
            }
        }
    }
}