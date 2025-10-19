using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        if (button != null) {
            button.onClick.AddListener(StartGame);
        } else {
            Debug.LogError("No Button component found on StartButton object!");
        }

        // Ensure game is paused at start
        Time.timeScale = 0f;

        AudioManager.instance?.StartTitleMusic();
    }

    void StartGame()
    {
        // Find and hide the TitleScreen parent
        Transform titleScreen = transform.parent;
        if (titleScreen != null && titleScreen.name == "TitleScreen")
        {
            titleScreen.gameObject.SetActive(false);
        } else {
            Debug.LogError("TitleScreen parent not found! Make sure the Button is a direct child of TitleScreen.");
            // Fallback - just hide this button
            gameObject.SetActive(false);
        }

        // Resume game time
        Time.timeScale = 1f;

        // Switch from title music to game music
        AudioManager.instance?.StopTitleMusic();
        AudioManager.instance?.StartMusic();
        
        // Initialize the game
        GameManager.singleton.InitializeGame(GameMode.Normal);
    }
}