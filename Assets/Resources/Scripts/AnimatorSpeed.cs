using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSpeed : MonoBehaviour {

	// Use this for initialization
	void Start () {
       gameObject.GetComponent<Animator>().speed = 0.1f;
        
    }
	
}
