using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievements : MonoBehaviour
{
    public Image medal;
    public Text txt;
    public string[] achievementList =
    {
        "Get to level 3",
        "Don't miss the first hoop!",
    };

    public int missed;


    public int[] completed =
    {
        0,
        0,
    };



    void Start()
    {
        missed = 0;
 
       // PlayerPrefs.SetInt("CompletedOne", 0);
       // PlayerPrefs.SetInt("CompletedTwo", 0);
        //
       // PlayerPrefs.SetInt("CompletedThree", 0);
        loadResults();
    }
    public void checkAllAchievements(int level)
    {
        for (int i = 0 ; i < achievementList.Length; i++)
        {
            checkAchievement(i, level);
        }
        checkCompleted();

    }


    public void checkCompleted()
    {
        int all = 0;
        for (int i = 0; i < achievementList.Length; i++)
        {
            if (completed[i] == 1)
                all++;
        }
        if (all == completed.Length)
        {
            Debug.Log("CONGRATS");
            medal.gameObject.SetActive(true);
            txt.gameObject.SetActive(false);
        }
        else
        {
            medal.gameObject.SetActive(false);
            txt.gameObject.SetActive(true);
        }

    }
    

    public void checkAchievement(int index, int level)
    {

        switch (index)
        {
            case 0:
                if (level >= 3)
                {
                    completed[0] = 1;
                    Debug.Log("Comp 1");
                }
                break;
            case 1:
                if (level == 2 && missed == 0)
                {
                    completed[1] = 1;

                }
                    
                break;
           

        }
        saveResults(); 

    }
    public void saveResults()
    {
        PlayerPrefs.SetInt("CompletedOne", completed[0]);
        PlayerPrefs.SetInt("CompletedTwo", completed[1]);


        PlayerPrefs.Save();
    }
    public void loadResults()
    {
        completed[0] = PlayerPrefs.GetInt("CompletedOne");
        completed[1] = PlayerPrefs.GetInt("CompletedTwo");

        updateText();
        checkCompleted();
        //completed[PlayerPrefs.GetInt("CompletedIndex")] = PlayerPrefs.GetInt("CompletedValue");

    }

    public void updateText()
    {
        
        // Achievements a = GameObject.Find("Score").GetComponent<Achievements>();
        //Text data = GameObject.Find("AchievementText").GetComponent<Text>();
        txt.text = "\n";
        for (int i = 0; i < achievementList.Length; i++)
        {
            string s = achievementList[i];
            Debug.Log("COMPLETED: " + completed[i]);
            if (completed[i] == 0)
            {
                txt.text += "• " + s + "\n";
            }
            else
            {
                txt.text += "• " + StrikeThrough(s) + "\n";
            }

        }


    }
    public string StrikeThrough(string s)
    {
        string strikethrough = "";
        foreach (char c in s)
        {
            strikethrough = strikethrough + c + '\u0336';
        }
        return strikethrough;
    }

}
