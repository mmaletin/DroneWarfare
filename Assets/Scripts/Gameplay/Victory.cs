
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour
{
    public static Victory instance { get; private set; }

    public GameObject victoryScreen;

    public bool visible = false;

    public float speed = 20;

    public List<Enemy> enemies;

    private void Awake()
    {
        instance = this;

        enemies = ComponentsFinder.Find<Enemy>();
    }

    private void Update()
    {
        CheckVictoryCondition();

        victoryScreen.transform.localScale = Vector3.Lerp(victoryScreen.transform.localScale, visible ? Vector3.one : Vector3.zero, speed * Time.unscaledDeltaTime);

        victoryScreen.SetActive(victoryScreen.transform.localScale.x > 0.01f);
    }

    internal void CheckVictoryCondition()
    {
        visible = true;

        foreach (var enemy in enemies)
            if (enemy != null)
            {
                visible = false;
                return;
            }

    }
}