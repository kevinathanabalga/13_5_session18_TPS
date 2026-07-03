using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private string menuScene = "MainMenu";

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(menuScene);
    }
}