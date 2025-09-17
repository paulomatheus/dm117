using UnityEngine;
using UnityEngine.SceneManagement;

public class StarMenuController : MonoBehaviour
{
    public void OnStartClick()
    {
        SceneManager.LoadScene("MainBuilding");
    }

    public void OnCreditsClick()
    {
        SceneManager.LoadScene("Credits");
    }


    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
