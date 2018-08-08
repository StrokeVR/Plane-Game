using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

using System.Linq;

public class KinectManager : MonoBehaviour {

    private KinectSensor _sensor;
    private BodyFrameReader _bodyFramereader;
    private Body[] _bodies = null;

    public bool isKinect;

    public static KinectManager instance = null;

    public GameObject rightHand;
    public GameObject leftHand;

    public Material def;
    public Material grab;

    public Body[] GetBodies()
    {
        return _bodies;
    }
    
	// Use this for initialization
	void Awake () {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

	}
	
	// Update is called once per frame
	void Start () {
        
        _sensor = KinectSensor.GetDefault();
        if (_sensor != null)
        {
            isKinect = _sensor.IsAvailable;
            _bodyFramereader = _sensor.BodyFrameSource.OpenReader();
            if (!_sensor.IsOpen)
            {
                _sensor.Open();
            }
            _bodies = new Body[_sensor.BodyFrameSource.BodyCount];
        }
	}
    void Update()
    {
        isKinect = _sensor.IsAvailable;

        if (_bodyFramereader != null)
        {
            var frame = _bodyFramereader.AcquireLatestFrame();
            if (frame != null)
            {
                frame.GetAndRefreshBodyData(_bodies); 
                foreach (var body in _bodies.Where(b => b.IsTracked))
                {
                    isKinect = true;
                    Debug.Log("Tracking");
                    if (body.HandRightConfidence == TrackingConfidence.High)
                    {
                        
                        Windows.Kinect.Joint handRight = body.Joints[JointType.HandRight];
                        rightHand.transform.localPosition = new Vector3(handRight.Position.X, handRight.Position.Y, handRight.Position.Z);
                        if (body.HandRightState == HandState.Open)
                        {
                            rightHand.GetComponent<MeshRenderer>().material = def;
                        }
                        else if (body.HandRightState == HandState.Closed)
                        {
                            rightHand.GetComponent<MeshRenderer>().material = grab;
                        }

                        GameObject.Find("Client").GetComponent<ClientController>().returnToClinician("kinectDataRight", "RightHand: {X: " +  handRight.Position.X + ", Y: " + handRight.Position.Y + ", Z: " + handRight.Position.Z + "}");
                    }
                    
                    if (body.HandLeftConfidence == TrackingConfidence.High)
                    {
                        Windows.Kinect.Joint handLeft = body.Joints[JointType.HandLeft];
                        leftHand.transform.localPosition = new Vector3(handLeft.Position.X, handLeft.Position.Y, handLeft.Position.Z);
                
                        if (body.HandLeftState == HandState.Open)
                        {
                            leftHand.GetComponent<MeshRenderer>().material = def;
                        }
                        else if (body.HandLeftState == HandState.Closed)
                        {
                            leftHand.GetComponent<MeshRenderer>().material = grab;
                        }
                        GameObject.Find("Client").GetComponent<ClientController>().returnToClinician("kinectDataLeft", "LeftHand: {X: " + handLeft.Position.X + ", Y: " + handLeft.Position.Y + ", Z: " + handLeft.Position.Z + "}");

                    }

                }
                frame.Dispose();
                frame = null;
            }
        }
    }
    void OnApplicationQuit()
    {
        if (_bodyFramereader != null)
        {
            _bodyFramereader.IsPaused = true;
            _bodyFramereader.Dispose();
            _bodyFramereader = null;
        }
        if (_sensor != null)
        {
            if (_sensor.IsOpen)
            {
                _sensor.Close();
            }
            _sensor = null;
        }
    }

}
