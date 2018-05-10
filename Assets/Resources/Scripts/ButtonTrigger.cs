using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTrigger : MonoBehaviour
{

    public float TriggerTime = 5f;
    public float CurrentTime = 0f;
    public bool BeingLookedAt = false;
    public Text Display;
    public Image Progress;

	// Use this for initialization
	void Start ()
    {
        Display = GetComponentInChildren<Text>();
        StartCoroutine(ResetTime());
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(BeingLookedAt)
        {
            CurrentTime += Time.deltaTime;
            CurrentTime = Mathf.Clamp(CurrentTime, 0f, TriggerTime);
        }
        else
        {
            if(CurrentTime >= 0)
            {
                CurrentTime -= Time.deltaTime;
                CurrentTime = Mathf.Clamp(CurrentTime, 0f, TriggerTime);
            }
            if(CurrentTime < 0)
            {
                CurrentTime = 0;
            }
        }
        Progress.fillAmount = CurrentTime/TriggerTime;
	}

    IEnumerator ResetTime()
    {
        while(true)
        {
            BeingLookedAt = false;
            yield return new WaitForSeconds(1f);
        }
    }
}
