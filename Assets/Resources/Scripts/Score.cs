using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public int level;

    public Text bestLevel;

    [SerializeField]
    private bool debugMode = true;
    

   //public DisplayScore ds;
    
    void Start()
    {
        //PlayerPrefs.SetFloat("bestTime", 0.0f);
        //achievements = GetComponent<Achievements>();
        //ds = GetComponent<DisplayScore>();
        //Debug.Log(achievements.gameObject.name);
        bestLevel = bestLevel.GetComponent<Text>();
        
        Load();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && debugMode == true)
        { 
            PlayerPrefs.SetInt("level", 1);
            PlayerPrefs.SetInt("CompletedOne", 0);
            PlayerPrefs.SetInt("CompletedTwo", 0);
            GameObject.Find("Achievements").GetComponent<Achievements>().loadResults();
            GameObject.Find("Achievements").GetComponent<Achievements>().checkCompleted();
            GameObject.Find("Achievements").GetComponent<Achievements>().updateText();

            Load();
        }
    }
	
     public void Save()
    {

        if (!PlayerPrefs.HasKey("level"))
        {
            PlayerPrefs.SetInt("level", this.level);
        }
        else
        {
            float prevLevel= PlayerPrefs.GetInt("level");
            Debug.Log(this.level + " < " + prevLevel);
            if (this.level > prevLevel)
            {
                PlayerPrefs.SetInt("level", this.level);
            }
           
        }
        
       

        PlayerPrefs.Save();
        Load();
    }

    public void Load()
    {
        
        if (PlayerPrefs.HasKey("level"))
        {
            this.level = PlayerPrefs.GetInt("level");
        }
        else
        {
            this.level = 1;
        }


        // Debug.Log(this.level); 
        bestLevel.text = "Best: " + this.level;
    }
   
}
