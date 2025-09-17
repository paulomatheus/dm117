using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    public void OnBackClick()
    {
        SceneManager.LoadScene("MainMenu");
        //UnityEngine.SceneManagement.SceneManager.LoadScene("StarMenu");
    }
}
