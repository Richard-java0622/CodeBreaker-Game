using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] protected GameObject player;

    [Header("UI")]
    [SerializeField] protected Text bustedText;
    [SerializeField] protected GameObject tutorialGUI;
    [SerializeField] protected Text moneyText;
    [SerializeField] protected GameObject restartUI;

    [Header("Money")]
    [SerializeField] protected int money = 0;

    protected AudioSource music;

    protected bool gameEnded = false;

    static GameSceneManager instance = null;

    static public GameSceneManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<GameSceneManager>();
            }

            return instance;
        }
    }

    void Start()
    {
        music = GetComponent<AudioSource>();

        if (music != null)
        {
            music.Play();
        }

        if (tutorialGUI != null)
        {
            tutorialGUI.SetActive(true);
        }

        if (restartUI != null)
        {
            restartUI.SetActive(false);
        }

        if (bustedText != null)
        {
            bustedText.gameObject.SetActive(false);
        }

        UpdateMoneyUI();
    }

    public void PlayerHit()
    {
        if (gameEnded)
        {
            return;
        }

        gameEnded = true;

        StartCoroutine(PlayerHitRoutine());
    }

    IEnumerator PlayerHitRoutine()
    {
        if (player != null)
        {
            Player playerScript = player.GetComponent<Player>();

            if (playerScript != null)
            {
                playerScript.SetMovementEnabled(false);
            }
        }

        yield return new WaitForSeconds(1.5f);

        if (player != null)
        {
            Destroy(player);
        }

        if (bustedText != null)
        {
            bustedText.gameObject.SetActive(true);
            bustedText.text = "Busted !!";
        }

        ShowRestartUI();
    }

    public void PlayerWon()
    {
        if (gameEnded)
        {
            return;
        }

        gameEnded = true;

        ShowRestartUI();
    }

    protected void ShowRestartUI()
    {
        if (restartUI != null)
        {
            restartUI.SetActive(true);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        Scene currentScene = SceneManager.GetActiveScene();

        SceneManager.LoadScene(currentScene.name);
    }

    public void AddMoney(int amount)
    {
        money += amount;

        UpdateMoneyUI();
    }

    void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = "$: " + money;
        }
    }

    public int GetMoney()
    {
        return money;
    }

    public void HideTutorial()
    {
        if (tutorialGUI != null)
        {
            tutorialGUI.SetActive(false);
        }
    }
}