using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurvivalModeButton : MonoBehaviour
{
    private Button button;
    
    [SerializeField] private GameObject titleScreen;

    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(StartSurvivalMode);
        }
        else
        {
            Debug.LogError("No Button component found on SurvivalModeButton object!");
        }
    }

    void StartSurvivalMode()
    {
        // Hide the title screen
        if (titleScreen != null)
        {
            titleScreen.SetActive(false);
        }
        else
        {
            // Find the TitleScreen parent if not assigned
            Transform parent = transform.parent;
            while (parent != null && parent.name != "TitleScreen")
            {
                parent = parent.parent;
            }
            
            if (parent != null)
            {
                parent.gameObject.SetActive(false);
            }
        }

        // Resume game time
        Time.timeScale = 1f;
        
        // Stop title music and start game music
        AudioManager.instance?.StopTitleMusic();
        
        // Initialize the game in survival mode
        GameManager.singleton.InitializeGame(GameMode.Survival);
    }
}