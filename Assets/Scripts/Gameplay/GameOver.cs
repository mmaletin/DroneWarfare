
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public static GameOver instance { get; private set; }

    public GameObject gameOverScreen;

    public bool visible = false;

    public float speed = 20;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Victory.instance.visible) visible = false;

        gameOverScreen.transform.localScale = Vector3.Lerp(gameOverScreen.transform.localScale, visible ? Vector3.one : Vector3.zero, speed * Time.unscaledDeltaTime);

        gameOverScreen.SetActive(gameOverScreen.transform.localScale.x > 0.01f);
    }
}