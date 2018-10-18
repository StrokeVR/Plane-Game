using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Data
{

    // public static float levelOfAssistance = 10;
    public static int level = 1;
    // public static float maxLevel = 5; //Inclusive
    public static int totalPlanes = 0;

    public static int planesHit = 0;
    public static int planesMissed = 0;
    // public static float levelMultiplier = 0.5f;
    public static float difficulty = 1f; 
    public static int levelToOscillate = 3; // -1 for never
    public static bool willOscillate = false;
    public static float oscillateSpeed = 0.05f;
    public static int minRange = -2;
    public static int maxRange = 2;

    public static Vector3 leftWrist;
    public static Vector3 rightWrist;
    public static Vector3 center;
    

    // Increment
    public static void incrementLevel()
    {
       
        planesMissed = totalPlanes - planesHit;
        Debug.Log("PH: " + planesHit + "; MS: " + planesMissed);   
        if (planesMissed == 0) planesMissed = -1;
        float percentPlanes = ((float)(totalPlanes - planesMissed) / totalPlanes);
        float newDifficulty = difficulty * percentPlanes;
        if (newDifficulty <= 0.02) newDifficulty = 0.02f;

        difficulty = newDifficulty;
        oscillateSpeed = difficulty / 30;
        if (difficulty >= 2)
        {
            willOscillate = true;
        }
        else
        {
            willOscillate = false;
        }
        
        int range = (int)difficulty;

        minRange = Random.Range(-range, -1);
        maxRange = Random.Range(0, range);
        Debug.Log("Diff:" + difficulty + ": " + minRange + "; " + maxRange);
        
    }

    public static void resetValues()
    {

        willOscillate = false;
        // float assist = levelOfAssistance + (maxLevel * levelMultiplier);
        // Debug.Log(assist);
        // setLevelOfAssistance(assist);
        level = 1;
        planesHit = 0;
        planesMissed = 0;
        difficulty = 1;
        oscillateSpeed = 0.1f;
    }
}
