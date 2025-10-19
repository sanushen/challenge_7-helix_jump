using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // CHANGED: Use TMPro instead of UnityEngine.UI

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text textScore;   // CHANGED: TMP_Text instead of Text
    [SerializeField] private TMP_Text textBest;    // CHANGED: TMP_Text instead of Text (Tutorial only uses normal text instead of text mesh pro which prevents dragging the text elements into the script slots)
    
    void Start()
    {
        UpdateUI();
    }
    
    void Update()
    {
        UpdateUI();
    }
    
    private void UpdateUI()
    {
        if(GameManager.singleton == null)
            return;
            
        if(textBest != null)
        {
            textBest.text = "Best: " + GameManager.singleton.best;
        }
        
        if(textScore != null)
        {
            textScore.text = "Score: " + GameManager.singleton.score;
        }
    }
}