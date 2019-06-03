
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private RectTransform rt;

    private void Start()
    {
        rt = transform as RectTransform;
    }

    private void Update()
    {
        rt.anchoredPosition = Input.mousePosition;
    }
}