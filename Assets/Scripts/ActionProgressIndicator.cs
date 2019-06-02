
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActionProgressIndicator : MonoBehaviour
{
    private static ActionProgressIndicator instance;

    private static Dictionary<Transform, ActionProgressIndicator> activeIndicators = new Dictionary<Transform, ActionProgressIndicator>();
    private static Queue<ActionProgressIndicator> disabledIndicators = new Queue<ActionProgressIndicator>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Init()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        instance = ComponentsFinder.FindSingle<ActionProgressIndicator>();

        if (instance == null)
            throw new NullReferenceException("There is no instance of ActionProgressIndicator on a scene");

        activeIndicators.Clear();
        disabledIndicators.Clear();

        barWidth = instance.bar.rect.width;

        disabledIndicators.Enqueue(instance);
    }

    private static void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        instance = ComponentsFinder.FindSingle<ActionProgressIndicator>();

        if (instance == null)
            throw new NullReferenceException("There is no instance of ActionProgressIndicator on a scene");

        activeIndicators.Clear();
        disabledIndicators.Clear();
    }

    public static void ShowFor(Transform target, float value)
    {
        ActionProgressIndicator indicator;

        // Get existing indicator for target or create a new one
        if (!activeIndicators.TryGetValue(target, out indicator))
        {
            if (disabledIndicators.Count != 0)
            {
                indicator = disabledIndicators.Dequeue();
            }
            else
            {
                indicator = Instantiate(instance, instance.transform.parent);
            }

            activeIndicators.Add(target, indicator);
            indicator.gameObject.SetActive(true);
        }

        indicator.SetValue(target, value);        
    }

    public static void HideFor(Transform target)
    {
        ActionProgressIndicator indicator;

        if (activeIndicators.TryGetValue(target, out indicator))
        {
            activeIndicators.Remove(target);
            disabledIndicators.Enqueue(indicator);

            indicator.gameObject.SetActive(false);
        }
    }

    public RectTransform bar;
    public Camera cam;

    private RectTransform rt;
    private static float barWidth;

    private void Awake()
    {
        rt = transform as RectTransform;
    }

    private void Reset()
    {
        cam = Camera.main;
    }

    private void SetValue(Transform target, float value)
    {
        rt.anchoredPosition = cam.WorldToScreenPoint(target.position) - new Vector3(Screen.width, Screen.height) * .5f;

        bar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, barWidth * value);
    }
}