using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour {
    public bool leftHand;
	
	
	// Update is called once per frame
	void Update () {
		if (leftHand)
        {
            gameObject.transform.localPosition = Data.leftWrist;
        }
        else
        {
            gameObject.transform.localPosition = Data.rightWrist;
        }
	}
}
