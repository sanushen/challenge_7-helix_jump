using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayAgainButton : MonoBehaviour
{
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(PlayAgain);
        }
    }

    void PlayAgain()
    {
        // Call the restart method in the GameManager
        if (GameManager.singleton != null)
        {
            GameManager.singleton.RestartGame();
        }
    }
}