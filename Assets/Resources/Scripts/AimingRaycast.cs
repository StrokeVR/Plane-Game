using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingRaycast : MonoBehaviour {
    private LineRenderer linerenderer;

    public void Start()
    {
        linerenderer = this.gameObject.GetComponent<LineRenderer>();
    }
	// Update is called once per frame
	void Update () {
        linerenderer.SetPositions(new Vector3[] {(this.gameObject.transform.position + this.gameObject.transform.forward / 2), (this.gameObject.transform.forward * 5 + this.gameObject.transform.position)});
	}
    void DrawGizmos()
    {

    }
}
 