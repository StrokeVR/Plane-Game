using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


namespace Assets.Resources.Scripts
{
    public class ClickButton : MonoBehaviour, IPointerExitHandler
    {
        public Level startLevel;
        public GameObject hitbox;
        public Canvas pointsdisplay;
        public Canvas reticlecanvas;
        // threshold to trigger button
        private float triggerTime = 3f;
        // current time hovered over button
        private float _currentTime = 0f;
        // start menu canvas
        public Canvas StartCanvas;
        // image of button
        public Image Progress;
        // bool to indicate if gaze is on the button
        public bool IsHovering = false;
        // gameobject that contains all of the menu buttons
        private GameObject _buttons;
        // array of all the menu buttons
        private Button[] _clickbuttons;
        public Camera Cam;

        void Start()
        {
            // collection of start menu buttons
            _buttons = GameObject.Find("Start Buttons");
            _clickbuttons = _buttons.GetComponentsInChildren<Button>();
            if (Data.level <= 1)
            {
                 foreach (Button b in _clickbuttons)
                {
                    if (b.gameObject.name.Equals("Exit"))
                    {
                        b.gameObject.SetActive(true);
                    }
                    else if (b.gameObject.name.Equals("Restart"))
                    {
                        b.gameObject.SetActive(false);
                    }

                }
            }
            else
            {
                foreach (Button b in _clickbuttons)
                {
                    if (b.gameObject.name.Equals("Restart"))
                    {
                        b.gameObject.SetActive(true);
                    }
                    else if (b.gameObject.name.Equals("Exit"))
                    {
                        b.gameObject.SetActive(false);
                    }

                }
            }
            hitbox.SetActive(false); 

            pointsdisplay.gameObject.SetActive(false);
            reticlecanvas.gameObject.SetActive(true);
            // show start menu canvas
            StartCanvas.gameObject.SetActive(true);
            StartCoroutine(ResetTime());
        }
        public void Update()
        {
            // if the gaze is on the button, calculate fill amount
            if (IsHovering)
            {
                _currentTime += Time.deltaTime;
                _currentTime = Mathf.Clamp(_currentTime, 0f, triggerTime);
            }
            // otherwise reduce fill amount
            else
            {
                if (_currentTime >= 0)
                {
                    _currentTime -= Time.deltaTime;
                    _currentTime = Mathf.Clamp(_currentTime, 0f, triggerTime);
                }
                if (_currentTime < 0)
                {
                    _currentTime = 0;
                }
            }
            // set fill amount to calculated amount
            Progress.fillAmount = _currentTime / triggerTime;
            if (Progress.fillAmount == 1)
            {
                // invoke the button click if completely filled
                gameObject.GetComponent<Button>().onClick.Invoke();
                _currentTime = 0;
            }
        }
        public void OnPointerExit(PointerEventData data)
        {
            IsHovering = false;
        }
        public void QuitGame()
        {
            // screen to black
            CameraClearFlags clear = CameraClearFlags.Color;
            Cam.clearFlags = clear;
            Cam.cullingMask = 0;
            // stop playing
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }

        public void LoadScene()
        {
            // screen to black
            CameraClearFlags clear = CameraClearFlags.Color;
            Cam.clearFlags = clear;

            Cam.cullingMask = 0;
            SceneManager.LoadScene("PlaneGame");
        }

        public void StartGame()
        {
            // spawn planes
            startLevel.SpawnInteractableObjects();
            // hide menu and reticle
            StartCanvas.gameObject.SetActive(false);
            reticlecanvas.gameObject.SetActive(false);
            // enable hitbox for throwing planes, and the points hud
            hitbox.SetActive(true);
            pointsdisplay.gameObject.SetActive(true);
        }

        public void RestartGame()
        {
            Data.resetValues();
            LoadScene();
        }

        IEnumerator ResetTime()
        {
            while (true)
            {
                IsHovering = false;
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
