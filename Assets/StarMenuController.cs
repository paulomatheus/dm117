using UnityEngine;
using UnityEngine.SceneManagement;

public class StarMenuController : MonoBehaviour
{
    public void OnStartClick()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
