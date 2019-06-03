
using System.Collections.Generic;
using UnityEngine;

// TODO Object pool?
public class Shot : MonoBehaviour
{
    public float speed = 10;

    public float maxDistance = 100;

    public GameObject owner;

    public GameObject hitEffect;

    public float shotLength;

    private List<Collider> colliders = new List<Collider>();

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        if (transform.position.sqrMagnitude > maxDistance * maxDistance)
            Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        IHitPoints targetWithHp = null;
        bool otherColliders = false;

        foreach (var collider in colliders)
        {
            if (collider == null) continue;

            if (targetWithHp == null)
                targetWithHp = collider?.GetComponentInParent<IHitPoints>();

            if (targetWithHp == null)
                otherColliders = true;
        }

        if (colliders.Count > 0)
        {
            Destroy(gameObject);
        }

        if (targetWithHp != null && !otherColliders)
        {
            targetWithHp.Hit();
            if (hitEffect != null)
                Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        colliders.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (owner != null && other.transform.IsChildOf(owner.transform)) return;

        colliders.Add(other);
    }

    private void OnDrawGizmosSelected()
    {
        AdditionalGizmos.DrawArrow(transform.position - transform.forward * shotLength * 0.5f, transform.forward * shotLength);
    }
}