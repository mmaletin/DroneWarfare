
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    private float scale = 0;

    public float time = 0.2f;

    void Update()
    {
        scale += Time.deltaTime / time;

        scale = Mathf.Clamp01(scale);

        transform.localScale = scale * Vector3.one;

        if (scale == 1)
            Destroy(gameObject);
    }
}