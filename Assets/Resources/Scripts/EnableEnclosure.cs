using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableEnclosure : MonoBehaviour {

    public GameObject Enclosure;
	
	public void Enable()
    {
        Enclosure.SetActive(true);
    }
    public void Disable()
    {
        Enclosure.SetActive(false);
    }
}
