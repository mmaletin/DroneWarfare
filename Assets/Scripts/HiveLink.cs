
using UnityEngine;

public class HiveLink : MonoBehaviour
{
    public Transform from, to;
    private LineRenderer lineRenderer;

    public float wobble = 0.1f;

    public bool active = false;

    public float decaySpeed = 5;

    private Color linkColor;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        linkColor = lineRenderer.material.color;

        lineRenderer.material.color = Color.black;
    }

    private void Update()
    {
        if (from != null && to != null && active)
        {
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                float t = (float)i / (lineRenderer.positionCount - 1);

                var pos = Vector3.Lerp(from.position, to.position, t) +
                    new Vector3(Random.Range(-wobble, wobble), Random.Range(-wobble, wobble), Random.Range(-wobble, wobble));

                lineRenderer.SetPosition(i, pos);
            }
        }

        lineRenderer.material.color =
            active ? linkColor :
            Color.Lerp(lineRenderer.material.color, Color.black, Time.deltaTime * decaySpeed);
    }
}
