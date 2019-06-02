
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadAction : MonoBehaviour
{
    public void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}