using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GameMode
{
    Normal,
    Survival,
    MultiBall,
}

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    public GameMode currentGameMode = GameMode.Normal;
    public int best;
    public int score;
    public int currentStage = 0;
    private bool gameInitialized = false;

    // Survival Mode
    public bool isSurvivalMode = false;
    public int highestSurvivalLevel = 0;

    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private int totalStages = 9;


    
    void Awake()
    {
        //Only allow a single game manager at all times
        if(singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);

        best = PlayerPrefs.GetInt("Highscore");

        //Hide gameover screen at start
        if(gameOverScreen != null)
            gameOverScreen.SetActive(false);
    }

    public void InitializeGame(GameMode mode)
    {
        // Only initialize once
        if (!gameInitialized)
        {
            gameInitialized = true;
            score = 0;
            currentStage = 0;
            currentGameMode = mode;

            if(currentGameMode == GameMode.Survival) {
                isSurvivalMode = true;
            }
            
            // Start the game by loading the first stage
            FindObjectOfType<BallController>().ResetBall();
            FindObjectOfType<HelixController>().LoadStage(currentStage);
            
            // Let the ball controller know game has started
            FindObjectOfType<BallController>().GameStarted();
            
            // Make sure game over screen is hidden
            if(gameOverScreen != null) {
                gameOverScreen.SetActive(false);
            }

            AudioManager.instance?.StartMusic();
        }
    }

    public void NextLevel()
    {
        if (!gameInitialized) return;        
        currentStage++;

        if (isSurvivalMode && currentStage > highestSurvivalLevel) {
            highestSurvivalLevel = currentStage;
            PlayerPrefs.SetInt("SurvivalLevel", highestSurvivalLevel);
        }
        
        // Check if we've completed all stages
        if (currentStage >= totalStages) {
            GameOver();
            return;
        }
        
        // Otherwise, load next stage
        FindObjectOfType<BallController>().ResetBall();
        FindObjectOfType<HelixController>().LoadStage(currentStage);
    }

    public void Restartlevel() {
        singleton.score = 0;

        // In Survival Mode, reset to stage 0
        if (isSurvivalMode) {
            currentStage = 0;
        }

        FindObjectOfType<BallController>().ResetBall();
        FindObjectOfType<HelixController>().LoadStage(currentStage);
    }

    public void AddScore(int scoreToAdd){
        score += scoreToAdd;
 
        if(score > best){
            best = score;
            PlayerPrefs.SetInt("Highscore", score);
        }
    }

    public void GameOver()
    {
        // Stop the ball
        BallController ball = FindObjectOfType<BallController>();
        if (ball != null && ball.rb != null) {
            ball.rb.isKinematic = true;
            ball.rb.velocity = Vector3.zero;
        }
        
        // Show game over screen
        if (gameOverScreen != null) {
            gameOverScreen.SetActive(true);
        }

        AudioManager.instance?.FadeOutMusic(1.5f);

        AudioManager.instance?.FadeInTitleMusic(1.5f);
    }

    public void RestartGame()
    {
        // Hide game over screen
        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);
            
        // Reset game state
        gameInitialized = false;
        if(isSurvivalMode) {
            InitializeGame(GameMode.Survival);
        } else {
            InitializeGame(GameMode.Normal);
        }
        
    }
}
