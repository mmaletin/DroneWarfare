
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    private void Start()
    {
#if UNITY_WEBGL
        gameObject.SetActive(false);
#endif
    }

    public void Quit()
    {
        Application.Quit();
    }
}