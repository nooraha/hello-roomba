using UnityEditor.Search;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    float gameLastedTimer;
    float maxGameLengthSeconds = 600f;
    bool currentlyPlaying = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (currentlyPlaying)
        {
            gameLastedTimer += Time.deltaTime;

            if (gameLastedTimer >= maxGameLengthSeconds)
            {
                WinGame();
            }
        }
    }
    
    public void OpenTitleScreen()
    {
        SceneManager.LoadScene("TitleScene");
    }
    public void StartGame()
    {
        gameLastedTimer = 0f;
        currentlyPlaying = true;
        SceneManager.LoadScene("SampleScene");
    }

    void CleanUpAfterGameEnd()
    {
        RoombaController.attackedPlayer.RemoveAllListeners();
    }

    public void WinGame()
    {
        // siwtch to victory screen scene to stop everything
        currentlyPlaying = false;
        CleanUpAfterGameEnd();
        SceneManager.LoadScene("VictoryScene");
        
    }
    
    public void LoseGame()
    {
        // switch to game over screen
        currentlyPlaying = false;
        CleanUpAfterGameEnd();
        SceneManager.LoadScene("GameOverScene");
    }
}
