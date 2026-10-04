using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{   
    [SerializeField] protected AudioSource mainMenuMusic;

    void Awake()
    {
        mainMenuMusic = GetComponent<AudioSource>();
        mainMenuMusic.Play();
    }
    public void StartGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}