using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinectObjectController : MonoBehaviour {

    public string type; //LeftHand, RightHand, etc
	// Use this for initialization
	// Update is called once per frame
	void Update () {
		switch (type)
        {
            case "Center":
                gameObject.transform.position = Data.center;
                break;
            case "LeftHand":

                break;
            case "RightHand":

                break;
        }
	}
}
