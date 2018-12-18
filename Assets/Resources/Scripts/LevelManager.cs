using UnityEngine;

namespace Assets.Resources.Scripts
{
    public class LevelManager : MonoBehaviour
    {
        public int LevelStart;
        public GameObject[] Levels;
        public OVRPlayerController Player;

        private int _currentLevel;
        private int[] _goals;
        private float[] _times;

        // called before Start
        public void Awake()
        {
            _currentLevel = LevelStart;
            _goals = new int[5];
            _times = new float[5];
            SetupScene(LevelStart);
        }

        // SetupScene initializes our level         
        public void SetupScene(int levelnum)
        {
            Level currLevel = Levels[levelnum].GetComponent<Level>();
            currLevel.enabled = true;
            // player placement
            SetPlayerPosition(levelnum);
            _goals[levelnum] = currLevel.Goal;
        }
 
        // new player position when level changed
        public void SetPlayerPosition(int levelnum)
        {
            Player.transform.position = Levels[levelnum].transform.position;
            Player.transform.rotation = Levels[levelnum].transform.rotation;
        }

        // disable previous level and enable the next one
        public void NextLevel()
        {
            _times[_currentLevel] = Levels[_currentLevel].GetComponent<Level>().GetTime();
            Levels[_currentLevel].GetComponent<Level>().enabled = false;
            _currentLevel++;
            Levels[_currentLevel].GetComponent<Level>().enabled = true;
            SetupScene(_currentLevel);
        }

        // getters

        public int[] GetGoals()
        {
            return _goals;
        }

        public float[] GetTimes()
        {
            return _times;
        }

        public GameObject GetNextLevel(int levelNum)
        {
            if (levelNum != Levels.Length - 1)
                return Levels[levelNum + 1];
            return Levels[levelNum];
        }

        public GameObject GetPreviousLevel(int levelNum)
        {
            if (levelNum != 0)
                return Levels[levelNum - 1];
            return Levels[levelNum];
        }

        public int GetCurrentLevel()
        {
            return _currentLevel;
        }
    }
}