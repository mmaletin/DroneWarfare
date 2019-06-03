
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance { get; private set; }

    public GameObject menuRoot;

    public bool visible = false;

    public float speed = 5;

    private void Awake()
    {
        instance = this;

        menuRoot.transform.localScale = visible ? Vector3.one : Vector3.zero;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            visible = !visible;
        }

        if (GameOver.instance.visible)
            visible = false;

        menuRoot.transform.localScale = Vector3.Lerp(menuRoot.transform.localScale, visible ? Vector3.one : Vector3.zero, speed * Time.unscaledDeltaTime);

        menuRoot.SetActive(menuRoot.transform.localScale.x > 0.01f);

        Time.timeScale = 1 - menuRoot.transform.localScale.x;
    }

    // For unity events
    public void SetVisible(bool value) => visible = value;
}