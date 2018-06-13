using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Data
{

    //public static float levelOfAssistance = 10;
    public static int level = 1;
    public static float maxLevel = 5; //Inclusive

    public static float levelMultiplier = 0.5f;
    public static int difficulty = 0;
    public static int levelToOscillate = 3; //-1 for never
    public static bool willOscillate = false;
    public static float oscillateSpeed = 0.05f;
    public static int minRange = -2;
    public static int maxRange = 2;
     


    //Increment
    public static void incrementLevel()
    {
        oscillateSpeed += 0.02f;
        
        if ((level % 2) == 0)
        {
            difficulty++;
        }
        if (difficulty > 4)
            difficulty = 0;

        int range = level;
        if (range > 5)
        {
            range = 5;
        } 
        minRange = Random.Range(-range, -1);
        maxRange = Random.Range(0, range);
        
    }

    public static void resetValues()
    {

        willOscillate = false;
        //float assist = levelOfAssistance + (maxLevel * levelMultiplier);
        //Debug.Log(assist);
        //setLevelOfAssistance(assist);
        level = 1;
        difficulty = 0;
        oscillateSpeed = 0.1f;
    }
}
