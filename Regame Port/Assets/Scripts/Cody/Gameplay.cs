﻿using System;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class Gameplay : MonoBehaviour
{
    public TutorialManager tutorialManager;
    public GameObject buttonPanel;
    public GameObject target;
    public GameObject platform;
    public TextMeshProUGUI instructionsText;
    public TextMeshProUGUI percentageOfThrows;
    public TextMeshProUGUI remainingThrows;
    public int totalThrowsAllowedPerLevel = 10;
    public AccuracyChecker accuracyChecker;
    public List<ButtonPressDetector> buttons = new List<ButtonPressDetector>();
    public int buttonsPressedTotal;
    public bool levelActivated = false;
    public LevelDifficulty levelDifficulty;
    public GameObject gameplayPanel;
    public GameplayTrigger gameplayTrigger;
    public GameObject percentagePanel;
    public GameObject instructionsPanel;
    public GameObject remainingThrowsPanel;
    public TextMeshProUGUI percentageText;
    public float completionPercent;
    public bool levelComplete = false;
    public ProjectileManager pm;
    public LogTestResults logTestResults;
    public GameObject completionPanel;
    public bool hasCompletedFinalLevel = false;
    public int numOfFinalRounds = 0;
    private bool gameComplete = false;
    public int totalLevelCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        accuracyChecker.ResetTotalThrows();
        buttonPanel.SetActive(true);
        instructionsText.text = "Great! Now we are ready to start the game. You have 30 chances to throw the ball. Try to get it in the basket! Press the blue button to start this level.";
        buttonsPressedTotal = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameComplete)
        {
            if (RoundCompleted() && !hasCompletedFinalLevel)
            {
                ResetButtons();
            }

            if (totalLevelCount == 6 && !hasCompletedFinalLevel)
            {
                hasCompletedFinalLevel = true;
                accuracyChecker.ResetTotalThrows();
                totalLevelCount = 0;
            }

            if (hasCompletedFinalLevel && totalLevelCount == 3)
            {
                Debug.Log("GAME OVER");
                instructionsPanel.SetActive(false);
                pm.ProjectileSwitch(false);
                target.SetActive(false);
                platform.SetActive(false);
                levelDifficulty.DestroyObstacle();
                remainingThrowsPanel.SetActive(false);
                completionPanel.SetActive(true);
                gameplayPanel.SetActive(false);
                gameplayPanel.SetActive(false);
                percentagePanel.SetActive(false);
                GetAccuracvPercentage();
                percentageText.text = "You got " + accuracyChecker.numHit + "/" + (accuracyChecker.numMiss + accuracyChecker.numHit) + " throws into the basket!";
                accuracyChecker.ResetTotalThrows();
                pm.buttonActivator = false;
                gameComplete = true;
                return;
            }

            LevelController();
        }
    }

    public void LevelController()
    {
        int total = accuracyChecker.TotalThrows();
        int throwsLeft = totalThrowsAllowedPerLevel - total;
        remainingThrows.text = "You have " + throwsLeft.ToString() + " left!";

        if (total >= totalThrowsAllowedPerLevel && hasCompletedFinalLevel && numOfFinalRounds < 4)
        {
            FinalRounds();
            Debug.Log("Final Round Completed");
        }
        
        if (total >= totalThrowsAllowedPerLevel && !hasCompletedFinalLevel)
        {
            LevelComplete();
        }
    }
    
    public void LevelComplete()
    {
        pm.ProjectileSwitch(false);
        target.SetActive(false);
        platform.SetActive(false);
        levelDifficulty.DestroyObstacle();
        instructionsPanel.SetActive(false);
        gameplayTrigger.ResetTrigger();
        gameplayPanel.SetActive(true);
        percentagePanel.SetActive(true);
        GetAccuracvPercentage();
        percentageText.text = "You got " + accuracyChecker.numHit + "/" + (accuracyChecker.numMiss + accuracyChecker.numHit) + " throws into the basket!";
        accuracyChecker.ResetTotalThrows();
        pm.buttonActivator = false;
        AddLevelCount();
    }

    public void FinalRounds()
    {
        pm.ProjectileSwitch(false);
        target.SetActive(false);
        platform.SetActive(false);
        levelDifficulty.DestroyObstacle();
        instructionsPanel.SetActive(false);
        gameplayTrigger.ResetTrigger();
        gameplayPanel.SetActive(true);
        percentagePanel.SetActive(true);
        GetAccuracvPercentage();
        percentageText.text = "You got " + accuracyChecker.numHit + "/" + (accuracyChecker.numMiss + accuracyChecker.numHit) + " throws into the basket!";
        accuracyChecker.ResetTotalThrows();
        pm.buttonActivator = false;
        AddLevelCount();
    }
    
    public int NumLevelsCompleted()
    {
        int totalButtonsPressed = 0;
        for(int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].buttonActivated)
            {
                totalButtonsPressed++;
            }
        }
        return totalButtonsPressed;
    }

    public void AddLevelCount()
    {
        totalLevelCount++;
    }

    public bool RoundCompleted()
    {
        if(NumLevelsCompleted() == 5)
        {
            return true;
        }
        return false;
    }

    public void GetAccuracvPercentage()
    {
        completionPercent = accuracyChecker.PercentageOfSuccess();
        completionPercent *= 100.0f;
    }

    public void ResetButtons()
    {
        for(int i = 0; i < buttons.Count; i++)
        {
            buttons[i].ResetButtonPosition();
        }
    }
}
