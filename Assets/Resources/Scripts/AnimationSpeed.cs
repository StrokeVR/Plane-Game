using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpeed : MonoBehaviour {
    public Animation anim;
	// Use this for initialization
	void Start () {
        anim["PlaneProp"].speed = 4.0f;
	}
	
}
