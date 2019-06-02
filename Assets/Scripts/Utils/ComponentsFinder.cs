
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ComponentsFinder {

    public static List<T> Find<T>()
    {
        List<T> components = new List<T>();

        int sceneCount = SceneManager.sceneCount;
        for (int i = 0; i < sceneCount; i++)
        {
            FindOnScene(SceneManager.GetSceneAt(i), components);
        }

        return components;
    }

    public static List<Component> Find(Type t)
    {
        List<Component> components = new List<Component>();

        int sceneCount = SceneManager.sceneCount;
        for (int i = 0; i < sceneCount; i++)
        {
            FindOnScene(SceneManager.GetSceneAt(i), t, components);
        }

        return components;
    }

    public static T FindSingle<T>() where T : class
    {
        T result = null;

        int sceneCount = SceneManager.sceneCount;
        for (int i = 0; i < sceneCount; i++)
        {
            result = FindOnSceneSingle<T>(SceneManager.GetSceneAt(i));
            if (result != null) return result;
        }

        return result;
    }

    public static Component FindSingle(Type t)
    {
        Component result = null;

        int sceneCount = SceneManager.sceneCount;
        for (int i = 0; i < sceneCount; i++)
        {
            result = FindOnSceneSingle(SceneManager.GetSceneAt(i), t);
            if (result != null) return result;
        }

        return result;
    }

    public static List<T> FindOnScene<T>(Scene scene, List<T> components = null)
    {
        if (components == null) components = new List<T>();

        var rootGameObjects = scene.GetRootGameObjects();

        foreach (var rootGameObject in rootGameObjects)
        {
            var rootGOComponents = rootGameObject.GetComponentsInChildren<T>(true);

            components.AddRange(rootGOComponents);
        }

        return components;
    }

    public static List<Component> FindOnScene(Scene scene, Type t, List<Component> components = null)
    {
        if (components == null) components = new List<Component>();

        var rootGameObjects = scene.GetRootGameObjects();

        foreach (var rootGameObject in rootGameObjects)
        {
            var rootGOComponents = rootGameObject.GetComponentsInChildren(t, true);

            components.AddRange(rootGOComponents);
        }

        return components;
    }

    public static T FindOnSceneSingle<T>(Scene scene) where T : class
    {
        var rootGameObjects = scene.GetRootGameObjects();

        foreach (var rootGameObject in rootGameObjects)
        {
            var component = rootGameObject.GetComponentInChildren<T>(true);

            if (component != null) return component;
        }

        return null;
    }

    public static Component FindOnSceneSingle(Scene scene, Type t)
    {
        var rootGameObjects = scene.GetRootGameObjects();

        foreach (var rootGameObject in rootGameObjects)
        {
            var component = rootGameObject.GetComponentInChildren(t, true);

            if (component != null) return component;
        }

        return null;
    }
}